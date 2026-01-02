using AlumniApi.Helpers;
using AlumniApi.Models;
using AlumniApi.Models.AlProfile;
using AlumniApi.Models.Caching;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace AlumniApi.Services.GeocodingService
{
    public class Geocoding : IGeocodingService
    {
        private readonly AlumniContext _context;
        private readonly HttpClient _httpClient;

        public Geocoding(AlumniContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<GeoCache> ResolveLocationAsync(string inputCity, Country country, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(inputCity))
                throw new ArgumentException("City is required.", nameof(inputCity));

            if (country is null)
                throw new ArgumentNullException(nameof(country));

            if (string.IsNullOrWhiteSpace(country.Name))
                throw new ArgumentException("Country.Name is required.", nameof(country));

            var city = inputCity.Trim();

            // 1) GENERIŠI KLJUČ
            var searchKey = StringHelper.GenerateSearchKey(city, country.Name);

            // 2) POKUŠAJ 1: BAZA (KEŠ)
            var cached = await _context.GeoCaches
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.SearchKey == searchKey, ct);

            if (cached != null) return cached;

            // 3) FALLBACK: koordinate države + IsVerified=false
            var newEntry = new GeoCache
            {
                City = city,
                Country = country.Name,
                SearchKey = searchKey,
                IsVerified = false,
                Latitude = country.DefaultLatitude,
                Longitude = country.DefaultLongitude
            };

            // 4) NOMINATIM API (ako uspije, pregazi fallback)
            try
            {
                if (!string.IsNullOrWhiteSpace(country.IsoCode))
                {
                    var url =
                        $"https://nominatim.openstreetmap.org/search" +
                        $"?city={Uri.EscapeDataString(city)}" +
                        $"&countrycodes={country.IsoCode.Trim().ToLowerInvariant()}" +
                        $"&format=json&limit=1&addressdetails=1";

                    using var response = await _httpClient.GetAsync(url, ct);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync(ct);
                        using var doc = JsonDocument.Parse(json);

                        if (doc.RootElement.ValueKind == JsonValueKind.Array &&
                            doc.RootElement.GetArrayLength() > 0)
                        {
                            var result = doc.RootElement[0];

                            if (result.TryGetProperty("lat", out var latProp) &&
                                result.TryGetProperty("lon", out var lonProp))
                            {
                                var latStr = latProp.GetString();
                                var lonStr = lonProp.GetString();

                                if (double.TryParse(latStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) &&
                                    double.TryParse(lonStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                                {
                                    newEntry.Latitude = lat;
                                    newEntry.Longitude = lon;
                                    newEntry.IsVerified = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
               
            }

            // 5) SAČUVAJ U BAZU
            _context.GeoCaches.Add(newEntry);
            await _context.SaveChangesAsync(ct);

            return newEntry;
        }
    }
}

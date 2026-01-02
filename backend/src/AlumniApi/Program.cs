builder.Services.AddHttpClient<IGeocodingService, Geocoding>(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd("AlumniApp/1.0 (contact: true_email@gmail.com)");
});

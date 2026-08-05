using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Seminario.Core.ArchivoManager.GoogleDrive;

public static class GoogleConfigurationService
{
    public static IServiceCollection AddGoogleDrive(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GoogleDriveOptions>(
            configuration.GetSection("GoogleDrive"));

        services.AddSingleton<DriveService>(sp =>
        {
            var options = sp.GetRequiredService<
                Microsoft.Extensions.Options.IOptions<GoogleDriveOptions>>().Value;

            var credential = GoogleCredential
                .FromFile(options.CredentialsPath)
                .CreateScoped(DriveService.Scope.Drive);

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Seminario"
            });
        });

        services.AddScoped<IArchivosManager, ArchivoGoogleDrive>();

        return services;
    }
}
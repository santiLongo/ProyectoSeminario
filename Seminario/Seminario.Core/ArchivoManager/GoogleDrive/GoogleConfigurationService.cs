using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            var options = sp.GetRequiredService<IOptions<GoogleDriveOptions>>().Value;

            using var stream = File.OpenRead(options.CredentialsPath);

            var secrets = GoogleClientSecrets
                .FromStream(stream)
                .Secrets;

            var flow = new GoogleAuthorizationCodeFlow(
                new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = secrets,
                    Scopes = new[]
                    {
                        DriveService.Scope.Drive
                    }
                });

            var credential = new UserCredential(
                flow,
                "default",
                new TokenResponse
                {
                    RefreshToken = options.RefreshToken
                });

            // Obtiene automáticamente un AccessToken usando el RefreshToken
            credential.RefreshTokenAsync(CancellationToken.None)
                .GetAwaiter()
                .GetResult();

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
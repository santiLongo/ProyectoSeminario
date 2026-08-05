using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Seminario.Core.Middleware.ExceptionMiddleware;
using Seminario.Core.Migrations.BaseMigrations;

namespace Seminario.Core.ConfigurationManager;

public static class RunAppManager
{
    public static async Task RunAplication(this WebApplication app)
    {
        using (var  scope = app.Services.CreateScope())
        {
            var migration = scope.ServiceProvider.GetRequiredService<BaseMigrations>();

            await migration.MigrarAsync();
        }

        app.UseMiddleware<ExceptionMiddleware>();

        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }

        app.UseCors(ConfigurationManager.CorsPolicy);

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
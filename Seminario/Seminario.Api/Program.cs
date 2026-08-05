using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Mapper;
using Seminario.Core.ArchivoManager.GoogleDrive;
using Seminario.Core.ConfigurationManager;
using Seminario.Core.ControlGroupSingleton;
using Seminario.Core.Dapper;
using Seminario.Core.Migrations.BaseMigrations;
using Seminario.Core.Services.CurrentUserService;
using Seminario.Datos.Contextos.SaveChangesInterceptors;
using Seminario.Datos.Migrations;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonServices(builder.Configuration, (services, configuration) =>
{
    services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
    {
        var auditService = sp.GetRequiredService<AuditSaveChangesInterceptor>();
        options.AddInterceptors(auditService);
        
        options.UseMySql(builder.Configuration.GetConnectionString("ConnectionMySql"),
            new MySqlServerVersion(new Version(9, 3, 0)));
    });
    //
    services.AddAutoMapper(typeof(MapperProfile));
    services.AddGoogleDrive(configuration);

    services.AddScoped<ICurrentUserService, CurrentUserService>();
    services.AddScoped<AuditSaveChangesInterceptor>();
    services.AddScoped<IDbSession, DbSession>();
    services.AddScoped<IDbExecutor, DbExecutor>();

    services.AddSingleton<IControlConnection, ControlGroupConnection>();

    services.AddTransient<BaseMigrations, Migration>();

    services.AddHttpClient("easyafip", c =>
    {
        c.BaseAddress = new Uri(configuration["EasyAfip:BaseUrl"]!);
        c.Timeout = TimeSpan.FromSeconds(90);
    });
});

var app = builder.Build();

await app.RunAplication();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Mapper;
using System.Text;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Api.Services.CurrentUserService;
using Seminario.Datos.Contextos.SaveChangesInterceptors;
using Seminario.Datos.ControlGroupSingleton;
using Seminario.Datos.Dapper;
using Seminario.Datos.Services.CurrentUserService;


var builder = WebApplication.CreateBuilder(args);

// Agrego los servicios que utiliza la app
// Agrego los controllers 
builder.Services.AddControllers();

// Agrego el context del Ef
builder.Services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
{
    var auditService = sp.GetRequiredService<AuditSaveChangesInterceptor>();
    options.AddInterceptors(auditService);
    
    options.UseMySql(builder.Configuration.GetConnectionString("ConnectionMySql"),
        new MySqlServerVersion(new Version(9, 3, 0)));
});

//Agrego el AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile));

//Agrego los Cors junto con sus variables de configuracion
var MiPoliticaCors = "_miPoliticaDeCors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MiPoliticaCors,
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()       
                .WithMethods("POST", "GET");
        });
});
//Agrego Autorizacion con el JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer =  false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("ApiSettings:Secreta").Value!))
    };
});
//Agrego el Autorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
//El Swagger seguro despues lo borro
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Implemento esto para poder acceder al header de la request
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
//Agrego en el Service las auditorias para el SaveChanges del Ef 
builder.Services.AddScoped<AuditSaveChangesInterceptor>();
//Agrego el IDbSession para el Dapper
builder.Services.AddScoped<IDbSession, DbSession>();
//Agrego singleton para las consultas a control group
builder.Services.AddSingleton<IControlConnection, ControlGroupConnection>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MiPoliticaCors);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

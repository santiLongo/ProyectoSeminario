using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Mapper;
using System.Text;
using Seminario.Datos.Dapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Agrego el context

builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
        options.UseMySql(builder.Configuration.GetConnectionString("ConnectionMySql"),
    new MySqlServerVersion(new Version(9, 3, 0))));

//Agrego el UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

var app = builder.Build();

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

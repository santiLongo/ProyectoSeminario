using EasyAfip.Core.Modelos;
using EasyAfip.Core.SoapClientService;
using EasyAfip.Service.FacturaElectronica;
using EasyAfip.Service.FacturaElectronica.Dummy;
using EasyAfip.Service.FacturaElectronica.UltimoComprobanteAutorizado;
using EasyAfip.Service.LoginWSAA;
using Seminario.Api.Middleware.ExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("open", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Configuración AFIP desde appsettings
builder.Services.Configure<AfipSettings>(builder.Configuration.GetSection("Afip"));

// HttpClient con timeout generoso para los WS de AFIP
builder.Services.AddHttpClient("afip", c => c.Timeout = TimeSpan.FromSeconds(60));

// ── Registros de servicios ──────────────────────────────────────────────────

// Singleton: cachea el ticket WSAA durante 12 hs, con lock para concurrencia
builder.Services.AddSingleton<IWsaaService, WsaaService>();

// Scoped: cada request obtiene su instancia
builder.Services.AddScoped<ISoapClientService, SoapClientService>();
builder.Services.AddScoped<IDummyService, DummyService>();
builder.Services.AddScoped<IUltimoComprobanteService, UltimoComprobanteService>();
builder.Services.AddScoped<IFeCAEService, FeCAEService>();

// ───────────────────────────────────────────────────────────────────────────

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("open");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

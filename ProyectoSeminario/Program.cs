using ProyectoSeminario.Services;
using ProyectoSeminario.Repository;
using ProyectoSeminario.Repository.IRepository;
using ProyectoSeminario.Mappers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Conexion a la base de datos
builder.Services.AddDbContext<AppDbContext>(opciones =>
                                            opciones.UseMySql(builder.Configuration.GetConnectionString("ConnectionMySql"),
                                                new MySqlServerVersion(new Version(9, 3, 0))));

//Agregamos los Repositorios
builder.Services.AddScoped<IRepositoryVehiculo, RepositoryVehiculo>();
builder.Services.AddScoped<IRepositoryUsuario, RepositoryUsuario>();

//Agregamos el AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

// Agregacion de Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuracion para evitar ciclos de referencia
builder.Services.AddControllers()
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilitacion de Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

//Falta configutar el enrutamiento predetermindado.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

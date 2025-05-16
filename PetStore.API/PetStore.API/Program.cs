using Microsoft.EntityFrameworkCore;
using PetStore.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura EF Core para usar MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PetStoreDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Swagger depende de esto
builder.Services.AddSwaggerGen();           // Aquí se registra Swagger

var app = builder.Build();

// ? Activar Swagger solo en entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Esto habilita /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

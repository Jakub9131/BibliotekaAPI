using BibliotekaAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------------
// JAWNA KONFIGURACJA HOSTÓW (Potwierdzona poprawnoœæ)
// --------------------------------------------------------
builder.WebHost.UseUrls("http://localhost:5000"); // TYLKO HTTP 5000

// --------------------------------------------------------
// KONFIGURACJA CORS (Potwierdzona poprawnoœæ)
// --------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// --------------------------------------------------------
// KONFIGURACJA BAZY DANYCH
// --------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(connectionString));

// --------------------------------------------------------
// KONFIGURACJA US£UG
// --------------------------------------------------------
builder.Services.AddControllers(); // KOREKTA: Usuniêto .AddJsonOptions(...)

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------------------------------------------
var app = builder.Build();

// --------------------------------------------------------
// PIPELINE APLIKACJI
// --------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
using DigitaltDodsbo.Core.Interfaces;
using DigitaltDodsbo.Infrastructure.Data;
using DigitaltDodsbo.Infrastructure.Repositories;
using DigitaltDodsbo.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Databas ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Data Source=digitaltdodsbo.db"));

// --- Dependency Injection ---
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();
builder.Services.AddScoped<IEstateCaseRepository, EstateCaseRepository>();
builder.Services.AddScoped<ILetterGenerator, TemplateLetterGenerator>();

// --- CORS för React-frontend (Vite default port 5173) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Kör migreringar automatiskt vid start (enkelt för utveckling)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();

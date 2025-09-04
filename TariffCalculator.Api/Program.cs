using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using TariffCalculator.Api.Data;
using TariffCalculator.Api.Infra;
using TariffCalculator.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    // Existing configs...
    c.OperationFilter<FormFileOperation>(); // Custom operation filter for IFormFile support
});

QuestPDF.Settings.License = LicenseType.Community;

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(conn))
{
    builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(conn));
}
builder.Services.AddSingleton<RulesEngineService>();
builder.Services.AddScoped<TariffCalculatorService>();
builder.Services.AddScoped<PdfReportService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("any", p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());
});
var app = builder.Build();
app.UseCors("any");

// Migrate or ensure created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch
    {
        db.Database.EnsureCreated();
    }
}
// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.Run();
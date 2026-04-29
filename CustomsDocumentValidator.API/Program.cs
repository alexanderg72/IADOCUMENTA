using CustomsDocumentValidator.Application.Interfaces;
using CustomsDocumentValidator.Application.Services;
using CustomsDocumentValidator.Infrastructure.OCR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IOcrService, AzureDocumentIntelligenceService>();
builder.Services.AddScoped<DocumentValidationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "swagger";
});
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", context =>
{
    context.Response.Redirect("/login.html");
    return Task.CompletedTask;
});

app.Run();
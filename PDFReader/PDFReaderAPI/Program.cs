using PDFReaderAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPdfsService, PdfsService>();

var apiKey = builder.Configuration["ApiKey"];
var apiUrl = builder.Configuration["ApiUrl"];

builder.Services.AddHttpClient("OCRApi", cfg =>
{
    cfg.DefaultRequestHeaders.Add("apikey", apiKey);
    cfg.BaseAddress = new Uri(apiUrl);
});

var app = builder.Build();

app.UseCors(cfg =>
{
    cfg.AllowAnyOrigin();
    cfg.AllowAnyMethod();
    cfg.AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

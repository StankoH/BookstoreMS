using BookstoreMS.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BookImporterService>();
builder.Services.AddHostedService<BookImportWorker>();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadOnly", policy =>
        policy.RequireRole("Read", "ReadWrite"));

    options.AddPolicy("ReadWrite", policy =>
        policy.RequireRole("ReadWrite"));
});

builder.Services.AddAuthentication("DummyScheme")
    .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("DummyScheme", options => { });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bookstore API",
        Version = "v10000000000000000000000",
        Description = "Tech zadatak",
        Contact = new OpenApiContact
        {
            Name = "đon do",
            Email = "nekimail@bonzobyte.com",
            Url = new Uri("https://bonzobyte.com")
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", context =>
    {
        context.Response.Redirect("/swagger");
        return Task.CompletedTask;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
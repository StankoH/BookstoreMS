using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BookstoreMS.Services
{
    public class BookImportWorker : BackgroundService
    {
        private readonly ILogger<BookImportWorker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _interval = TimeSpan.FromHours(1);

        public BookImportWorker(
            ILogger<BookImportWorker> logger,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("start import");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var importer = scope.ServiceProvider.GetRequiredService<BookImporterService>();

                    var relativePath = _configuration["ImportSettings:JsonFilePath"];
                    var absolutePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", relativePath));

                    if (!File.Exists(absolutePath))
                    {
                        _logger.LogError("missing json main file: {Path}", absolutePath);
                    }
                    else
                    {
                        _logger.LogInformation("Imort book from: {Path}", absolutePath);
                        await importer.ImportBooksFromJsonAsync(absolutePath);
                        _logger.LogInformation("Import completed");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Nesto se skurvalo");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
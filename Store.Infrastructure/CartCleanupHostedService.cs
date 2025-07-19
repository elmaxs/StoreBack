using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Application.Abstractions.User;

namespace Store.Infrastructure
{
    public class CartCleanupHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(5);

        public CartCleanupHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var cleanupService = scope.ServiceProvider.GetRequiredService<ICartService>();

                try
                {
                    await cleanupService.CleanExpiredCartsAsync();
                }
                catch (Exception ex)
                {
                    // Логування помилки (за потреби можна підключити ILogger)
                    Console.WriteLine($"[CartCleanup] Error: {ex.Message}");
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}

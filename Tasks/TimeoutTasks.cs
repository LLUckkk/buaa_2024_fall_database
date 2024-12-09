using Market.Interfaces;
using Market.Services;

namespace Market.Tasks
{
    public class TimeoutTasks(ILogger<TimeoutTasks> logger, IServiceProvider serviceProvider) : IHostedService, IDisposable
    {
        private readonly ILogger<TimeoutTasks> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private Timer _timer { get; set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Saving database changes.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
                dbContext.PaymentPays.Where(p => p.ProcessStatus == 1 && p.PaymentStatus == 9).ToList().ForEach(p =>
                    paymentService.PayStatusUpdateCallback(p)
                );
                dbContext.PaymentOrders.Where(o => o.ProcessStatus == 1 && o.PaymentStatus == 9).ToList().ForEach(o =>
                    paymentService.PaymentOrderStatusUpdateCallback(o)
                );
                var productOrderService = scope.ServiceProvider.GetRequiredService<IProductOrderService>();
                dbContext.ProductOrders.Where(o => o.DealStatus == 2 && o.CreateTime.AddMinutes(10) < DateTime.UtcNow).ToList().ForEach(o =>
                    productOrderService.ClearOutdatedOrder(o)
                );
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SaveDatabaseChangesTask is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
using Market.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Market.Tasks
{
    public class SaveDatabaseChangesTask : IHostedService, IDisposable
    {
        private readonly ILogger<SaveDatabaseChangesTask> _logger;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public SaveDatabaseChangesTask(ILogger<SaveDatabaseChangesTask> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Saving database changes.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.SaveChanges();
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
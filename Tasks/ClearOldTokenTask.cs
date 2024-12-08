using Market.Interfaces;

public class ClearOldTokenTask : IHostedService, IDisposable
{
    private readonly ILogger<ClearOldTokenTask> _logger;
    private readonly ITokenService _tokenService;
    private Timer _timer;

    public ClearOldTokenTask(ILogger<ClearOldTokenTask> logger, ITokenService tokenService)
    {
        _logger = logger;
        _tokenService = tokenService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        _logger.LogInformation("Clearing old tokens.");
        _tokenService.ClearOldToken();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ClearOldTokenTask is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

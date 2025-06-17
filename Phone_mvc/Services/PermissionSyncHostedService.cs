namespace Phone_mvc.Services
{
    public class PermissionSyncHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PermissionSyncHostedService> _logger;

        public PermissionSyncHostedService(IServiceProvider serviceProvider, ILogger<PermissionSyncHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();
            _logger.LogInformation("Starting permission synchronization at {Time}", DateTime.Now);
            await permissionService.SyncPermissionsAsync();
            _logger.LogInformation("Permission synchronization completed at {Time}", DateTime.Now);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}

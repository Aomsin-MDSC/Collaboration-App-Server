using System.Diagnostics;

namespace CollaborationAppAPI.Services
{
    public class AnnouncementCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AnnouncementCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Debug.WriteLine("AnnouncementCleanupService is starting...");
            while (!stoppingToken.IsCancellationRequested)
            {
                Debug.WriteLine("AnnouncementCleanupService: Waiting...");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var announcementService = scope.ServiceProvider.GetRequiredService<AnnouncementService>();
                    Debug.WriteLine("Background Service: Cleaning up expired announcements...");
                    await announcementService.DeleteExpiredAnnouncementsAsync();
                }
            }
        }
    }

}

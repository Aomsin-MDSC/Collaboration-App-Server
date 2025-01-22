using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CollaborationAppAPI.Models;
using Google;
using Microsoft.EntityFrameworkCore;

namespace CollaborationAppAPI.Services
{
    public class AnnouncementService
    {
        private readonly AppDbContext _context;

        public AnnouncementService(AppDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteExpiredAnnouncementsAsync()
        {
            var localNow = DateTime.Now;
            Debug.WriteLine($"Current UTC time: {localNow}");

            var expiredAnnouncements = await _context.Announces
                 .Where(a => a.Announce_date < localNow)
                .ToListAsync();

            Debug.WriteLine($"Found {expiredAnnouncements.Count} expired announcements.");

            foreach (var announcement in expiredAnnouncements)
            {
                Debug.WriteLine($"Deleting announcement with ID {announcement.Announce_id} and date {announcement.Announce_date}");
                _context.Announces.Remove(announcement);
            }
            Debug.WriteLine($"Deleting {expiredAnnouncements.Count} expired announcements.");
            await _context.SaveChangesAsync();
        }
    }
}

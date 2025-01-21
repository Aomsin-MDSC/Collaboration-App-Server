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
            var expiredAnnouncements = await _context.Announces
                .Where(a => EF.Functions.DateDiffSecond(a.Announce_date, DateTime.UtcNow) <= 0)
                .ToListAsync();

            Debug.WriteLine($"Found {expiredAnnouncements.Count} expired announcements.");

            foreach (var announcement in expiredAnnouncements)
            {
                _context.Announces.Remove(announcement);
            }

            await _context.SaveChangesAsync();
        }
    }
}

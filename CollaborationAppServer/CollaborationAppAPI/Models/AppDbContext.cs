using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CollaborationAppAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }

}

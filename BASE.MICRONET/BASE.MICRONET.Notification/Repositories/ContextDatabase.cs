using Microsoft.EntityFrameworkCore;
using BASE.MICRONET.Notification.Models;

namespace BASE.MICRONET.Notification.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }

        public DbSet<SendMail> SendMail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SendMail>().ToTable("SendMail");
        }
    }
}

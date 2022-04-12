using Microsoft.EntityFrameworkCore;
using BASE.MICRONET.Withdrawal.Models;

namespace BASE.MICRONET.Withdrawal.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Transaction>().ToTable("Transaction");
        }
    }
}

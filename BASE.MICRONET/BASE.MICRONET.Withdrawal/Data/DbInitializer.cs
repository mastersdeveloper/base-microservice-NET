using BASE.MICRONET.Withdrawal.Repositories;

namespace BASE.MICRONET.Withdrawal.Data
{
    public class DbInitializer
    {
        public static void Initialize(ContextDatabase context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}

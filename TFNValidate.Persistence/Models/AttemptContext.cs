using Microsoft.EntityFrameworkCore;
namespace TFNValidate.Persistence.Models
{
    public class AttemptContext : DbContext
    {
        public AttemptContext(DbContextOptions<AttemptContext> options) : base(options) { }

        public DbSet<Attempt> Attempts { get; set; }
    }
}

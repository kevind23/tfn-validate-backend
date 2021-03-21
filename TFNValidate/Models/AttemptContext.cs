using Microsoft.EntityFrameworkCore;
namespace TFNValidate.API.Models
{
    public class AttemptContext : DbContext
    {
        public AttemptContext(DbContextOptions<AttemptContext> options) : base(options) { }

        public DbSet<Attempt> Attempts { get; set; }
    }
}

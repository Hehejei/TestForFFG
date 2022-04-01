using Microsoft.EntityFrameworkCore;

namespace TestForFFG.Models
{
    public class DbCntx : DbContext
    {
        public DbCntx(DbContextOptions<DbCntx> options) : base(options)
        {}

        public DbSet<Obj> objects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

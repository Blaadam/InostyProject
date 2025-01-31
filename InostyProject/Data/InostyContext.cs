using InostyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace InostyApp.Data
{
    public class InostyContext: DbContext
    {
        public InostyContext(DbContextOptions<InostyContext> options) : base(options)
        {
        }

        public DbSet<Account> UserTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Account>().To
        }

    }
}

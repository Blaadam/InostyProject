using InostyProject.Models;
using Microsoft.EntityFrameworkCore;

namespace InostyApp.Data
{
    public class InostyContext : DbContext
    {
        public InostyContext(DbContextOptions<InostyContext> options) : base(options)
        {
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Workspace> WorkspaceTable { get; set; }
        public DbSet<MemberLink> MemberTable { get; set; }
        public DbSet<Board> BoardTable { get; set; }
        public DbSet<BoardList> ListTable { get; set; }
        public DbSet<Card> CardTable { get; set; }
    }
}

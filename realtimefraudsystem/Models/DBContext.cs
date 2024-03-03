using Microsoft.EntityFrameworkCore;

namespace realtimefraudsystem.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
    }
}

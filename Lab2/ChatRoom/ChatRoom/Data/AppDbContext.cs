using ChatRoom.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatRoom.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=chatroom.db");
        }
    }
}
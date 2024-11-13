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
        public DbSet<ChatRoomEntity> ChatRooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlite("Data Source=chatroom.db");
            options.UseSqlServer("Server=172.20.0.2,1433,1433;Database=ChatApp;User Id=sa;Password=YourStrong!Password;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatRoomEntity>()
                .HasMany(c => c.Users)
                .WithMany(u => u.ChatRooms);

            modelBuilder.Entity<User>()
                .HasMany(x => x.ChatRooms)
                .WithMany(x => x.Users);

            base.OnModelCreating(modelBuilder);
        }
    }
}
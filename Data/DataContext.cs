using Microsoft.EntityFrameworkCore;
using two_realities.Models;

namespace two_realities.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }
        
        public DbSet<User>? Users { get; set; }
        public DbSet<FavoritePair>? FavoritePairs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(p => p.UserId);
                entity.Property(p => p.PasswordHash);
                entity.Property(p => p.PasswordSalt);
            });

            modelBuilder.Entity<FavoritePair>(entity =>
            {
                entity.HasKey(p => p.UserId);
                entity.Property(p => p.TitleOne);
                entity.Property(p => p.YearOne);
                entity.Property(p => p.TitleTwo);
                entity.Property(p => p.YearTwo);    
            });
        }
    }
}

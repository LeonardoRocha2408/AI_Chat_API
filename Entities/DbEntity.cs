using Microsoft.EntityFrameworkCore;

namespace ChatBot.Entities
{
    public class DbEntity : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessagesEntity>()
                .Property(m => m.Role)
                .HasConversion<string>();
        }


        public DbEntity(DbContextOptions options) : base(options) { }
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<MessagesEntity> Messages { get; set; }
        public DbSet<UserEntity> Users { get; set; } 
    }
}

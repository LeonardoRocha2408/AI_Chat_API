using Microsoft.EntityFrameworkCore;

namespace ChatBot.Entities
{
    public class DbEntity : DbContext
    {
        public DbEntity(DbContextOptions options) : base(options) { }
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<MessagesEntity> Messages { get; set; }
        public DbSet<UserEntity> Users { get; set; } 
    }
}

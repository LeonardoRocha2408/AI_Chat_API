using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Entities
{
    [Table("Conversations")]
    public class ConversationEntity
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; init; }
        [Column("Title")]
        public string Title { get; init; } = string.Empty;
        [Column("UserName")]
        public string UserName { get; init; } = string.Empty;
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; init; }
    }
}

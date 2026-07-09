using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using ChatBot.Enums;

namespace ChatBot.Entities
{
    [Table("Messages")]
    public class MessagesEntity
    {
        [Column("Id")]
        public Guid Id { get; init; }
        [Column("ConversationId")]
        public Guid ConversationId { get; init; }
        [Column("Role")]
        public Role Role { get; init; }
        [Column("Content")]
        public string Content { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}

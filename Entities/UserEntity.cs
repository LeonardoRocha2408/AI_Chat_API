using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Entities
{
    [Table("Users")]
    public class UserEntity
    {
        [Column("Id")]
        public Guid Id { get; set; }
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;
        [Column("Password")]
        public string Password { get; set; } = string.Empty;
        [Column("CreatedAt")]
        DateTime CreatedAt { get; set; }
    }
}

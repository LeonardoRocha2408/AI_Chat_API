using EnumShared.Enums;
namespace DTOs
{
    public class ChatResponse
    {
        public Guid ConversationId { get; set; }

        public Guid UserMessageId { get; set; }

        public Guid AssistantMessageId { get; set; }

        public string title { get; set; } = string.Empty;

        public string content { get; set; } = string.Empty;
    };
}

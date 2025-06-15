namespace NeuroTumAI.Core.Dtos.Chat
{
	public class MessageToReturnDto
	{
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public int ConversationId { get; set; }
        public DateTime SentAt { get; set; }
    }
}

using System;

namespace MyChat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SendingDateTime { get; set; }

        public Message(int id, int chatId, int senderId, string content, DateTime sendingDateTime)
        {
            Id = id;
            ChatId = chatId;
            SenderId = senderId;
            Content = content;
            SendingDateTime = sendingDateTime;
        }
    }
}

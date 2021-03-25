namespace MyChat.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string Path { get; set; }

        public Attachment(int id, int messageId, string path)
        {
            Id = id;
            MessageId = messageId;
            Path = path;
        }
    }
}

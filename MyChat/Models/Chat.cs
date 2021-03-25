namespace MyChat.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatorId { get; set; }

        public Chat(int id, string name, int creatorId)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
        }
    }
}

using MyChat.Databases;
using MyChat.Repositories;

namespace MyChat.Services
{
    public class ApplicationDataService
    {
        public AttachmentRepository Attachments { get; private set; }
        public ChatRepository Chats { get; private set; }
        public MessageRepository Messages { get; private set; }
        public UserRepository Users { get; private set; }

        public ApplicationDataService(IDatabase database)
        {
            Attachments = new AttachmentRepository(database);
            Chats = new ChatRepository(database);
            Messages = new MessageRepository(database);
            Users = new UserRepository(database);
        }
    }
}

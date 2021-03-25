using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MyChat.Databases;
using MyChat.Models;

namespace MyChat.Repositories
{
    public class MessageRepository : AbstractRepository<Message>
    {
        public MessageRepository(IDatabase database) : base(database) { }

        public List<Message> GetMessages()
        {
            string sqlExpression = "SELECT * FROM `messages`";

            return GetList(sqlExpression);
        }

        public Message GetMessageById(int messageId)
        {
            string sqlExpression = "SELECT * FROM `messages` WHERE `message_id` = @message_id LIMIT 1;";

            DbParameter messageIdParameter = BuildParameter("message_id", messageId);

            return GetFirst(sqlExpression, messageIdParameter);
        }

        public List<Message> GetChatMessages(Chat chat)
        {
            if (chat == null)
            {
                return new List<Message>();
            }

            return GetChatMessagesByChatId(chat.Id);
        }

        public List<Message> GetChatMessagesByChatId(int chatId)
        {
            string sqlExpression = "SELECT * FROM `messages` WHERE `chat_id` = @chat_id;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            return GetList(sqlExpression, chatIdParameter);
        }

        public Message GetChatMessage(Chat chat, Message message)
        {
            if (chat == null || message == null)
            {
                return null;
            }

            return GetChatMessageByChatIdAndMessageId(chat.Id, message.Id);
        }

        public Message GetChatMessageByChatIdAndMessageId(int chatId, int messageId)
        {
            string sqlExpression = "SELECT * FROM `messages` WHERE `message_id` = @message_id AND `chat_id` = @chat_id LIMIT 1;";

            DbParameter messageIdParameter = BuildParameter("message_id", messageId);
            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            return GetFirst(sqlExpression, messageIdParameter, chatIdParameter);
        }

        public override int Add(Message message)
        {
            if (message == null)
            {
                return 0;
            }

            string sqlExpression = "INSERT INTO `messages` (`message_id`, `chat_id`, " +
                "`sender_id`, `content`, `sending_datetime`) VALUES (@message_id, " +
                "@chat_id, @sender_id, @content, @sending_datetime);";

            DbParameter messageIdParameter = BuildParameter("message_id", message.Id);
            DbParameter chatIdParameter = BuildParameter("chat_id", message.ChatId);
            DbParameter senderIdParameter = BuildParameter("sender_id", message.SenderId);
            DbParameter contentParameter = BuildParameter("content", message.Content);
            DbParameter sendingDateTimeParameter = BuildParameter("sending_datetime", message.SendingDateTime);

            int generatedId = ExecuteInsert(sqlExpression, messageIdParameter,
                chatIdParameter, senderIdParameter, contentParameter, 
                sendingDateTimeParameter);

            return generatedId;
        }

        public override int Update(Message message)
        {
            if (message == null)
            {
                return 0;
            }

            string sqlExpression = "UPDATE `messages` SET `chat_id` = @chat_id, " +
                "`sender_id` = @sender_id, `content` = @content, " +
                "`sending_datetime` = @sending_datetime " +
                "WHERE `message_id` = @message_id;";

            DbParameter messageIdParameter = BuildParameter("message_id", message.Id);
            DbParameter chatIdParameter = BuildParameter("chat_id", message.ChatId);
            DbParameter senderIdParameter = BuildParameter("sender_id", message.SenderId);
            DbParameter contentParameter = BuildParameter("content", message.Content);
            DbParameter sendingDateTimeParameter = BuildParameter("sending_datetime", message.SendingDateTime);

            int updatedRowsCount = ExecuteUpdate(sqlExpression, messageIdParameter, 
                chatIdParameter, senderIdParameter, contentParameter,
                sendingDateTimeParameter);

            return updatedRowsCount;
        }

        public override int Remove(Message message)
        {
            if (message == null)
            {
                return 0;
            }

            return RemoveById(message.Id);
        }

        public int RemoveById(int messageId)
        {
            string sqlExpression = "DELETE FROM `messages` " +
                "WHERE `message_id` = @message_id;";

            DbParameter messageIdParameter = BuildParameter("message_id", messageId);

            int removedRowsCount = ExecuteUpdate(sqlExpression, messageIdParameter);

            return removedRowsCount;
        }

        protected override Message ParseDataRow(DataRow row)
        {
            int messageId = (int)row["message_id"];
            int chatId = (int)row["chat_id"];
            int senderId = (int)row["sender_id"];
            string content = (string)row["content"];
            DateTime sendingDateTime = (DateTime)row["sending_datetime"];

            return new Message(messageId, chatId, senderId, content, sendingDateTime);
        }
    }
}

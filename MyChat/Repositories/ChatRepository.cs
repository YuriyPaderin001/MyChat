using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MyChat.Databases;
using MyChat.Models;

namespace MyChat.Repositories
{
    public class ChatRepository : AbstractRepository<Chat>
    {
        public ChatRepository(IDatabase database) : base(database) { }

        public List<Chat> GetChats()
        {
            string sqlExpression = "SELECT * FROM `chats`";

            return GetList(sqlExpression);
        }

        public Chat GetChatById(int chatId)
        {
            string sqlExpression = "SELECT * FROM `chats` WHERE `chat_id` = @chat_id;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            return GetFirst(sqlExpression, chatIdParameter);
        }

        public List<Chat> GetUserChats(User user)
        {
            if (user == null)
            {
                return new List<Chat>();
            }

            return GetUserChatsByUserId(user.Id);
        }

        public List<Chat> GetUserChatsByUserId(int userId)
        {
            string sqlExpression = "SELECT `ch`.* FROM `chats` AS `ch` " +
                "INNER JOIN `chats_members` AS `chm`" +
                "ON `chm`.`chat_id` = `ch`.`chat_id`" +
                "WHERE `user_id` = @user_id;";

            DbParameter userIdParameter = BuildParameter("user_id", userId);

            return GetList(sqlExpression, userIdParameter);
        }

        public override int Add(Chat chat)
        {
            if (chat == null)
            {
                return 0;
            }

            string sqlExpression = "INSERT INTO `chats` (`chat_id`, `name`, " +
                "`creator_id`) VALUES (@chat_id, @name, @creator_id);";

            DbParameter chatIdParameter = BuildParameter("chat_id", chat.Id);
            DbParameter nameParameter = BuildParameter("name", chat.Name);
            DbParameter creatorIdParameter = BuildParameter("creator_id", chat.CreatorId);

            int generatedId = ExecuteInsert(sqlExpression, chatIdParameter,
                nameParameter, creatorIdParameter);

            return generatedId;
        }

        public int AddUserToChat(User user, Chat chat)
        {
            if (user == null || chat == null)
            {
                return 0;
            }

            return AddUserToChatByUserIdAndChatId(user.Id, chat.Id);
        }

        public int AddUserToChatByUserIdAndChatId(int userId, int chatId)
        {
            string sqlExpression = "INSERT INTO `chats_members` (`user_id`, `chat_id`) VALUES(@user_id, @chat_id);";

            DbParameter userIdParameter = BuildParameter("user_id", userId);
            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            int addedRowsCount = ExecuteUpdate(sqlExpression, userIdParameter, chatIdParameter);

            return addedRowsCount;
        }

        public override int Update(Chat chat)
        {
            if (chat == null)
            {
                return 0;
            }

            string sqlExpression = "UPDATE `chats` SET `name` = @name, " +
                "`creator_id` = @creator_id WHERE `chat_id` = @chat_id;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chat.Id);
            DbParameter nameParameter = BuildParameter("name", chat.Name);
            DbParameter creatorIdParameter = BuildParameter("creator_id", chat.CreatorId);

            int updatedRowsCount = ExecuteUpdate(sqlExpression, chatIdParameter,
                nameParameter, creatorIdParameter);

            return updatedRowsCount;
        }

        public override int Remove(Chat chat)
        {
            if (chat == null)
            {
                return 0;
            }

            return RemoveById(chat.Id);
        }

        public int RemoveById(int chatId)
        {
            string sqlExpression = "DELETE FROM `chats` WHERE `chat_id` = @chat_id;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            int removedRowsCount = ExecuteUpdate(sqlExpression, chatIdParameter);

            return removedRowsCount;
        }

        public int RemoveUserFromChat(User user, Chat chat)
        {
            if (user == null || chat == null)
            {
                return 0;
            }

            return RemoveUserFromChatByUserIdAndChatId(user.Id, chat.Id);
        }

        public int RemoveUserFromChatByUserIdAndChatId(int userId, int chatId)
        {
            string sqlExpression = "DELETE FROM `chats_members` WHERE `user_id` = @user_id AND `chat_id` = @chat_id;";

            DbParameter userIdParameter = BuildParameter("user_id", userId);
            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            int removedRowsCount = ExecuteUpdate(sqlExpression, userIdParameter, chatIdParameter);

            return removedRowsCount;
        }

        protected override Chat ParseDataRow(DataRow row)
        {
            int chatId = (int)row["chat_id"];
            string name = (string)row["name"];
            int creatorId = (int)row["creator_id"];

            return new Chat(chatId, name, creatorId);
        }
    }
}

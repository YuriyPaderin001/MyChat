using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using MyChat.Databases;
using MyChat.Models;

namespace MyChat.Repositories
{
    public class UserRepository : AbstractRepository<User>
    {
        public UserRepository(IDatabase database) : base(database) { }

        public List<User> GetUsers()
        {
            string sqlExpression = "SELECT * FROM `users`;";

            return GetList(sqlExpression);
        }

        public User GetUserById(int userId)
        {
            string sqlExpression = "SELECT * FROM `users` WHERE `user_id` = @user_id LIMIT 1;";

            DbParameter userIdParameter = BuildParameter("user_id", userId);

            return GetFirst(sqlExpression, userIdParameter);
        }

        public User GetUserByLoginData(LoginData loginData)
        {
            if (loginData == null)
            {
                return null;
            }

            string sqlExpression = "SELECT * FROM `users` WHERE `login` = @login AND `password` = @password LIMIT 1;";

            string login = HashFunction(loginData.Login);
            DbParameter loginParameter = BuildParameter("login", login);

            string password = HashFunction(loginData.Password);
            DbParameter passwordParameter = BuildParameter("password", password);

            return GetFirst(sqlExpression, loginParameter, passwordParameter);
        }

        public User GetUserByLogin(string login)
        {
            string sqlExpression = "SELECT * FROM `users` WHERE `login` = @login LIMIT 1;";

            DbParameter loginParameter = BuildParameter("login", login);

            return GetFirst(sqlExpression, loginParameter);
        }

        public List<User> GetChatMembers(Chat chat)
        {
            if (chat == null)
            {
                return new List<User>();
            }

            return GetChatMembersByChatId(chat.Id);
        }

        public List<User> GetChatMembersByChatId(int chatId)
        {
            string sqlExpression = "SELECT `usr`.* FROM `users` AS `usr` " +
                "INNER JOIN `chats_members` AS `chm` " +
                "ON `chm`.`user_id` = `usr`.`user_id` " +
                "WHERE `chm`.`chat_id` = @chat_id;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            return GetList(sqlExpression, chatIdParameter);
        }

        public User GetChatMember(Chat chat)
        {
            if (chat == null)
            {
                return null;
            }

            return GetChatMemberByChatId(chat.Id);
        }

        public User GetChatMemberByChatId(int chatId)
        {
            string sqlExpression = "SELECT `usr`.* FROM `users` AS `usr` " +
                "INNER JOIN `chats_members` AS `chm` " +
                "ON `chm`.`user_id` = `usr`.`user_id` " +
                "WHERE `chm`.`chat_id` = @chat_id LIMIT 1;";

            DbParameter chatIdParameter = BuildParameter("chat_id", chatId);

            return GetFirst(sqlExpression, chatIdParameter);
        }

        public override int Add(User user)
        {
            if (user == null)
            {
                return 0;
            }

            try
            {
                user.Login = HashFunction(user.Login);
                user.Password = HashFunction(user.Password);
            }
            catch (ArgumentNullException ex)
            {
                Console.Error.WriteLine(ex);

                return 0;
            }

            string sqlExpression = "INSERT INTO `users` (`user_id`, `first_name`, " +
                "`middle_name`, `last_name`, `login`, `password`) VALUES (" +
                "@user_id, @first_name, @middle_name, @last_name, @login, " +
                "@password);";

            DbParameter userIdParameter = BuildParameter("user_id", user.Id);
            DbParameter firstNameParameter = BuildParameter("first_name", user.FirstName);
            DbParameter middleNameParameter = BuildParameter("middle_name", user.MiddleName);
            DbParameter lastNameParameter = BuildParameter("last_name", user.LastName);
            DbParameter loginParameter = BuildParameter("login", user.Login);
            DbParameter passwordParameter = BuildParameter("password", user.Password);

            int addedRowsCount = ExecuteUpdate(sqlExpression, userIdParameter, firstNameParameter, 
                middleNameParameter, lastNameParameter, loginParameter, passwordParameter);

            return addedRowsCount;
        }

        public override int Update(User user)
        {
            if (user == null)
            {
                return 0;
            }

            try
            {
                user.Login = HashFunction(user.Login);
                user.Password = HashFunction(user.Password);
            }
            catch (ArgumentNullException ex)
            {
                Console.Error.WriteLine(ex);

                return 0;
            }

            string sqlExpression = "UPDATE `users` SET `first_name` = @first_name, " +
                "`middle_name` = @middle_name, `last_name` = @last_name, " +
                "`login` = @login, `password` = @password WHERE `user_id` = @user_id;";

            DbParameter userIdParameter = BuildParameter("user_id", user.Id);
            DbParameter firstNameParameter = BuildParameter("first_name", user.FirstName);
            DbParameter middleNameParameter = BuildParameter("middle_name", user.MiddleName);
            DbParameter lastNameParameter = BuildParameter("last_name", user.LastName);
            DbParameter loginParameter = BuildParameter("login", user.Login);
            DbParameter passwordParameter = BuildParameter("password", user.Password);

            int updatedRowsCount = ExecuteUpdate(sqlExpression, userIdParameter, firstNameParameter,
                middleNameParameter, lastNameParameter, loginParameter, passwordParameter);

            return updatedRowsCount;
        }

        public override int Remove(User user)
        {
            if (user == null)
            {
                return 0;
            }

            return RemoveById(user.Id);
        }

        public int RemoveById(int userId)
        {
            string sqlExpression = "DELETE FROM `users` WHERE `user_id` = @user_id;";

            DbParameter userIdParameter = BuildParameter("user_id", userId);

            int removedRowsCount = ExecuteUpdate(sqlExpression, userIdParameter);

            return removedRowsCount;
        }

        protected override User ParseDataRow(DataRow row)
        {
            int userId = (int)row["user_id"];
            string firstName = (string)row["first_name"];
            string middleName = (string)row["middle_name"];
            string lastName = (string)row["last_name"];
            string login = (string)row["login"];
            string password = (string)row["password"];

            return new User(userId, firstName, middleName, lastName, login, password);
        }

        private string HashFunction(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            byte[] salt = new byte[] { 12, 23, 34, 45, 67, 89, 54, 32 };
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(source, salt, 3))
            {
                byte[] key = bytes.GetBytes(salt.Length);

                return Convert.ToBase64String(key);
            }
        }
    }
}

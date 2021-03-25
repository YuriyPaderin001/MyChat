using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MyChat.Databases;
using MyChat.Models;

namespace MyChat.Repositories
{
    public class AttachmentRepository : AbstractRepository<Attachment>
    {
        public AttachmentRepository(IDatabase database) : base(database) { }

        public List<Attachment> GetAttachments()
        {
            string sqlExpression = "SELECT * FROM `messages_attachments`";

            return GetList(sqlExpression);
        }

        public Attachment GetAttachmentById(int attachmentId)
        {
            string sqlExpression = "SELECT * FROM `messages_attachments` WHERE `attachment_id` = @attachment_id LIMIT 1;";

            DbParameter attachmentIdParameter = BuildParameter("attachment_id", attachmentId);

            return GetFirst(sqlExpression, attachmentIdParameter);
        }

        public Attachment GetMessageAttachmentById(Message message, int attachmentId)
        {
            if (message == null)
            {
                return null;
            }

            return GetMessageAttachmentByMessageIdAndAttachmentId(message.Id, attachmentId);
        }

        public Attachment GetMessageAttachmentByMessageIdAndAttachmentId(int messageId, int attachmentId)
        {
            string sqlExpression = "SELECT * FROM `messages_attachments` WHERE `attachemnt_id` = @attachment_id " +
                "AND `message_id` = @message_id LIMIT 1;";

            DbParameter attachmentIdParameter = BuildParameter("attachment_id", attachmentId);
            DbParameter messageIdParameter = BuildParameter("message_id", messageId);

            return GetFirst(sqlExpression, attachmentIdParameter, messageIdParameter);
        }

        public List<Attachment> GetMessageAttachments(Message message)
        {
            if (message == null)
            {
                return new List<Attachment>();
            }

            return GetMessageAttachmentsByMessageId(message.Id);
        }

        public List<Attachment> GetMessageAttachmentsByMessageId(int messageId)
        {
            string sqlExpression = "SELECT * FROM `messages_attachments` " +
                "WHERE `message_id` = @message_id;";

            DbParameter messageIdParameter = BuildParameter("message_id", messageId);

            return GetList(sqlExpression, messageIdParameter);
        }

        public override int Add(Attachment attachment)
        {
            if (attachment == null)
            {
                return 0;
            }

            string sqlExpression = "INSERT INTO `messages_attachments` (`attachment_id`, " +
                "`message_id`, `path`) VALUES (@attachment_id, @message_id, @path);";

            DbParameter attachmentIdParameter = BuildParameter("attachment_id", attachment.Id);
            DbParameter messageIdParameter = BuildParameter("message_id", attachment.MessageId);
            DbParameter pathParameter = BuildParameter("path", attachment.Path);

            int generatedId = ExecuteInsert(sqlExpression, attachmentIdParameter,
                messageIdParameter, pathParameter);

            return generatedId;
        }

        public override int Update(Attachment attachment)
        {
            if (attachment == null)
            {
                return 0;
            }

            string sqlExpression = "UPDATE `messages_attachments` " +
                "SET `message_id` = @message_id, `path` = @path " +
                "WHERE `attachment_id` = @attachment_id;";

            DbParameter attachmentIdParameter = BuildParameter("attachment_id", attachment.Id);
            DbParameter messageIdParameter = BuildParameter("message_id", attachment.MessageId);
            DbParameter pathParameter = BuildParameter("path", attachment.Path);

            int updatedRowsCount = ExecuteUpdate(sqlExpression, attachmentIdParameter,
                messageIdParameter, pathParameter);

            return updatedRowsCount;
        }

        public override int Remove(Attachment attachment)
        {
            if (attachment == null)
            {
                return 0;
            }

            return RemoveById(attachment.Id);
        }

        public int RemoveById(int attachmentId)
        {
            string sqlExpression = "DELETE FROM `messages_attachments` WHERE `attachment_id` = @attachment_id;";

            DbParameter attachmentIdParameter = BuildParameter("attachment_id", attachmentId);

            int removedRowsCount = ExecuteUpdate(sqlExpression, attachmentIdParameter);

            return removedRowsCount;
        }

        protected override Attachment ParseDataRow(DataRow row)
        {
            int attachmentId = (int)row["attachment_id"];
            int messageId = (int)row["message_id"];
            string path = (string)row["path"];

            return new Attachment(attachmentId, messageId, path);
        }
    }
}

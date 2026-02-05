using System;

namespace DataAccessLayer.Models
{
    public class SentEmail
    {
        public int SentEmailId { get; set; }

        #region Email Content
        public string SentTo { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public DateTime SentAt { get; set; }
        public string BccEmails { get; set; }
        public string CcEmails { get; set; }
        public string ReplyTo { get; set; }
        public string SentFrom { get; set; }
        public string Username { get; set; }

        // Don't think we'll need attachments!
        //public IEnumerable<FileUpload> Attachments { get; set; }
        #endregion
    }
}

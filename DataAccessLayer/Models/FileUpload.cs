using System;

namespace DataAccessLayer.Models
{
    public class FileUpload
    {
        public int FileUploadId { get; set; }
        public string FileName { get; set; }
        public string ReadableFileName { get; set; }
        public byte[] File { get; set; }
        public DateTime FileUploadDateTime { get; set; }
        public string FileHash { get; set; }
        public string UploadedBy { get; set; }

        // Foreign Key
        public int WorkRequestId { get; set; }
    }
}

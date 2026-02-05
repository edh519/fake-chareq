using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    [Table("DataExportLog", Schema = "dbo")]
    public class DataExportLog
    {
        public int Id { get; set; }
        public int? WorkRequestId { get; set; }
        public string DownloadName { get; set; }
        public string DataExportBy { get; set; }
        public DateTime DataExportDate { get; set; }
        public bool DataExportSuccessful { get; set; }
        public string ExceptionText { get; set; }
    }
}

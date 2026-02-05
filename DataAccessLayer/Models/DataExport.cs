using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models;

public class DataExportJob
{
    public int Id { get; set; }
    public int TrialId { get; set; }
    public Trial Trial { get; set; }
    public string RequestedById { get; set; }
    public ApplicationUser RequestedBy { get; set; }

    public DateTime RequestedAt { get; set; }

    [ForeignKey(nameof(Status))]
    public DataExportStatusEnum StatusId { get; set; }
    public DataExportStatus Status { get; set; }
    public string FilePath { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool NotificationSent { get; set; }
}

public enum DataExportStatusEnum
{
    Queued,
    Processing,
    Completed,
    Failed
}
using DataAccessLayer.Models;
using System;

namespace BusinessLayer.ViewModels;

public class DataExportJobViewModel
{
    public int Id { get; set; }
    public string TrialName { get; set; }
    public DateTime RequestedAt { get; set; }
    public DataExportStatusEnum StatusEnum { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool CanDownload { get; set; }
    public string FileName { get; set; }

    public string Status => StatusEnum.ToString();

    public string StatusCssClass
    {
        get
        {
            return StatusEnum switch
            {
                DataExportStatusEnum.Queued => "bg-secondary",
                DataExportStatusEnum.Processing => "bg-primary",
                DataExportStatusEnum.Completed => "bg-success",
                DataExportStatusEnum.Failed => "bg-danger",
                _ => "bg-dark",
            };
        }
    }

    public string ExpiredOrNoDataExported
    {
        get
        {
            return StatusEnum switch
            {
                DataExportStatusEnum.Queued => "In Queue",
                DataExportStatusEnum.Processing => "Processing...",
                DataExportStatusEnum.Failed => "Export Failed",
                DataExportStatusEnum.Completed => "File Expired",
                _ => "Unavailable"
            };
        }
    }

    public string StatusMessageCssClass => StatusEnum switch
    {
        DataExportStatusEnum.Processing => "text-primary italic",
        DataExportStatusEnum.Failed => "text-danger",
        _ => "text-muted"
    };
}
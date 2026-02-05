namespace DataAccessLayer.Models;

public class DataExportStatus
{
    public DataExportStatusEnum DataExportStatusId { get; set; }
    public string DataExportStatusName { get; set; }
    public bool IsActive { get; set; }
}
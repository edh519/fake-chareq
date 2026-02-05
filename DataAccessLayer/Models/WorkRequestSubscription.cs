namespace DataAccessLayer.Models;

public class WorkRequestSubscription
{
    public int Id { get; set; }
    public WorkRequest WorkRequest { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
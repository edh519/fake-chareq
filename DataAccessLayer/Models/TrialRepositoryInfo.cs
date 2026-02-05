using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class TrialRepositoryInfo
{
    [Key]
    public int Id { get; set; }
    public long GitHubRepositoryId { get; set; }
    public int TrialId { get; set; }
    public Trial Trial { get; set; }
}
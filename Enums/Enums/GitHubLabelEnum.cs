using System.ComponentModel.DataAnnotations;

namespace Enums.Enums;

public enum GitHubLabelEnum
{
    [Display(Name = "to discuss")]
    ToDiscuss,
    [Display(Name = "ChaReq")]
    ChaReq,
    [Display(Name = "approval needed")]
    ApprovalNeeded
}
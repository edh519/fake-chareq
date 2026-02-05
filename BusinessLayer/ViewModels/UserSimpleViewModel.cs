using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessLayer.ViewModels;

public class UserSimpleViewModel
{
    public string UserId { get; set; }
    [Required]
    public string Email { get; set; }
    public string Username { get; set; }
    [Display(Name = "ChaReq Role")]
    public string RoleAsString { get; set; }
    public int RoleId { get; set; }
    public List<SelectListItem> Roles { get; set; }
    public bool IsLocked { get; set; }
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [DisplayName("Full Name")]
    public string FullName { get; set; }
    public bool IsSystemAccount { get; set; }
}
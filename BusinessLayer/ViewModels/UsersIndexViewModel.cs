using System.Collections.Generic;

namespace BusinessLayer.ViewModels;

public class UsersIndexViewModel
{
    public List<UserSimpleViewModel> Users { get; set; } = new();
    public bool SystemHasInactiveUsers { get; set; }
}
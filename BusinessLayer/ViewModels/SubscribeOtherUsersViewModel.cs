using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
  public class SubscribeOtherUsersViewModel
  {
    public int WorkRequestId { get; set; }
    public List<UserSimpleViewModel> SubscribableUsers { get; set; }
    [Required(ErrorMessage = "User to subscribe is required")]
    public List<string> UsersToBeSubscribed { get; set; }
    public string CurrentUserId { get; set; }
  }
}

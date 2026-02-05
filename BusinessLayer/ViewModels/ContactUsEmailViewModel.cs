using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class ContactUsEmailViewModel
{
    public ContactUsEmailViewModel(ContactUs contact)
    {
        Message = contact.Message;
        Email = contact.Email;
        Submitted = contact.Submitted;
    }

    public string Message { get; set; }
    public string Email { get; set; }
    public DateTime Submitted { get; set; }
    public string LinkToSystem { get; set; }
}

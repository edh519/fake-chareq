using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories;

public class ContactUsRepository : Repository<ContactUs>, IContactUsRepository
{
    public ContactUsRepository(ApplicationDbContext context) : base(context)
    { }

    public void AddContactUsSubmission(ContactUs contactUsSubmission)
    {
        Insert(contactUsSubmission);
        Save();
    }

}

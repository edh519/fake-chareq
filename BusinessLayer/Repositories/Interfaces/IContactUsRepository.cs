using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories.Interfaces;

public interface IContactUsRepository : IRepository<ContactUs>
{
    void AddContactUsSubmission(ContactUs contactUsSubmission);
}

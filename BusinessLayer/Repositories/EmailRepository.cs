using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class EmailRepository : Repository<SentEmail>, IEmailRepository
    {
        public EmailRepository(ApplicationDbContext context) : base(context)
        { }

        }
}

using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repositories;

public class TriageInfoRotaRepository : Repository<TriageInfoRota>, ITriageInfoRotaRepository
{
    public TriageInfoRotaRepository(ApplicationDbContext context) : base(context)
    { }

    public List<TriageInfoRota> GetAllRotas()
    {
        return _context.TriageInfoRotas.ToList();
    }
}

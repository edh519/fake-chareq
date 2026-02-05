using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces;

public interface ITriageInfoRotaRepository : IRepository<TriageInfoRota>
{
    List<TriageInfoRota> GetAllRotas();
}
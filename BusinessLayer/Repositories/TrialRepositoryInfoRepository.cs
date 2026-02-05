using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;

namespace BusinessLayer.Repositories;

public class TrialRepositoryInfoRepository : Repository<TrialRepositoryInfo>, ITrialRepositoryInfoRepository
{
    public TrialRepositoryInfoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public List<TrialRepositoryInfo> GetByTrialId(int trialId)
    {
        return _context.TrialRepositoryInfos
            .Where(i => i.TrialId == trialId)
            .ToList();
    }

    public TrialRepositoryInfo GetByGitHubRepositoryId(long gitHubRespositoryId)
    {
        return _context.TrialRepositoryInfos
            .FirstOrDefault(i => i.GitHubRepositoryId == gitHubRespositoryId);
    }
}
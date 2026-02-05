using BusinessLayer.Repositories.Interfaces;
using System.Collections.Generic;
using DataAccessLayer.Models;

public interface ITrialRepositoryInfoRepository : IRepository<TrialRepositoryInfo>
{
    List<TrialRepositoryInfo> GetByTrialId(int trialId);
    TrialRepositoryInfo GetByGitHubRepositoryId(long gitHubRespositoryId);
}
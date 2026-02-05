using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface ITrialRepository : IRepository<Trial>
    {
        List<Trial> GetActiveTrials();
        List<Trial> GetRedcapTrials();
        List<Trial> GetDevelopmentTrials();
        List<Trial> GetNotAttributedTrials();
        int AddTrial(Trial trial);
        int ArchiveTrial(int trialId);
        int UnarchiveLabel(int trialId);
        Trial GetByIdIncludeRepositoryInfo(int id);
        bool DeleteConfiguredRepository(int trialId, long repositoryId);
        bool TrialRepositoryHasAssociatedWorkRequests(int trialId, long repositoryId);
        List<Trial> GetTrialsWithClosedOrAbandonedWorkRequests();
    }
}

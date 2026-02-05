using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using Enums.Enums;
using Microsoft.EntityFrameworkCore;
using Trial = DataAccessLayer.Models.Trial;

namespace BusinessLayer.Repositories
{
    public class TrialRepository : Repository<Trial>, ITrialRepository
    {
        public TrialRepository(ApplicationDbContext context) : base(context)
        { }


        public List<Trial> GetActiveTrials()
        {
            return _context.Trials
                    .Where(i => i.IsActive)
                    .ToList();
        }

        public List<Trial> GetRedcapTrials()
        {
            return _context.Trials
                .Where(i => i.TrialAttribution == Enums.Enums.TrialAttribution.REDCap)
                .ToList();
        }
        public List<Trial> GetDevelopmentTrials()
        {
            return _context.Trials
                .Where(i => i.TrialAttribution == Enums.Enums.TrialAttribution.Development)
                .ToList();
        }
        public List<Trial> GetNotAttributedTrials()
        {
            return _context.Trials
                .Where(i => !i.TrialAttribution.HasValue)
                .ToList();
        }

        public int AddTrial(Trial trial)
        {
            trial.IsActive = true;
            _context.Add(trial);
            _context.SaveChanges();
            return trial.TrialId;
        }

        public int ArchiveTrial(int trialId)
        {
            Trial trial = GetByID(trialId);
            trial.IsActive = true;
            _context.SaveChanges();
            return trialId;
        }

        public int UnarchiveLabel(int trialId)
        {
            Trial trial = GetByID(trialId);
            trial.IsActive = false;
            _context.SaveChanges();
            return trialId;
        }
        public Trial GetByIdIncludeRepositoryInfo(int id)
        {
            return _context.Trials
                .Include(e => e.TrialRepositoryInfos)
                .Include(t => t.TrialEmail)
                .FirstOrDefault(e => e.TrialId == id);
        }

        public bool DeleteConfiguredRepository(int trialId, long repositoryId)
        {
            DataAccessLayer.Models.TrialRepositoryInfo item = _context.TrialRepositoryInfos.FirstOrDefault(e =>
                e.TrialId == trialId && e.GitHubRepositoryId == repositoryId);
            if (item is not null)
            {
                _context.TrialRepositoryInfos.Where(e => e.Id == item.Id)
                    .ExecuteDelete();

                return true;
            }
            else
            {
                return false; 
            }
        }

        public bool TrialRepositoryHasAssociatedWorkRequests(int trialId, long repositoryId)
        {
            return _context.WorkRequests.Any(e => e.Trial.TrialId == trialId && e.AssignedTrialRepositoryId == repositoryId);
        }

        public List<Trial> GetTrialsWithClosedOrAbandonedWorkRequests()
        {
            return _context.WorkRequests.AsNoTracking()
                .Include(e=>e.Status)
                .Include(wr => wr.Trial)
                .Where(wr => wr.Status.WorkRequestStatusId == WorkRequestStatusEnum.Completed || wr.Status.WorkRequestStatusId == WorkRequestStatusEnum.Abandoned)
                .Select(wr => wr.Trial)
                .Distinct()
                .OrderBy(e => e.IsActive)
                .ToList();
        }
    }
}

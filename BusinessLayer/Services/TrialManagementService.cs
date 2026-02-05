using BusinessLayer.Repositories;
using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class TrialManagementService : ITrialManagementService
    {
        private readonly ITrialRepository _trialRepository;
        private readonly GitHubService _gitHubService;
        private readonly ILogger<TrialManagementService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserUpdateService _userUpdateService;

        public TrialManagementService(ITrialRepository trialRepository, GitHubService gitHubService, ILogger<TrialManagementService> logger, UserManager<ApplicationUser> userManager, IUserUpdateService userUpdateService)
        {
            _trialRepository = trialRepository;
            _gitHubService = gitHubService;
            _logger = logger;
            _userManager = userManager;
            _userUpdateService = userUpdateService;
        }

        public async Task<TrialViewModel?> GetTrialViewModelAsync(int trialId)
        {
            Trial trial = _trialRepository.GetByIdIncludeRepositoryInfo(trialId);
            if (trial == null)
            {
                return null;
            }

            TrialViewModel trialViewModel = new(trial);

            List<Repository> configuredGitRepositories = new();
            try
            {
                configuredGitRepositories = await _gitHubService.GetRepositories(trial.TrialRepositoryInfos.Select(e => e.GitHubRepositoryId).ToArray());
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex, $"Could not configure Git Repositories for Trial {trial.TrialName}");
            }
           
            IEnumerable<ConfiguredTrialRepositoyViewModel> configuredTrialRepositoryViewModels = configuredGitRepositories.Select(e => new ConfiguredTrialRepositoyViewModel()
            {
                Repository = e,
                HasAssociatedWorkRequests = _trialRepository.TrialRepositoryHasAssociatedWorkRequests(trialId, e.Id)
            });

            trialViewModel.ConfiguredRepositories = configuredTrialRepositoryViewModels.ToList();
            return trialViewModel;
        }

        public void UpdateTrialDetails(TrialViewModel trialViewModel)
        {
            Trial trial = _trialRepository.GetByIdIncludeRepositoryInfo(trialViewModel.TrialId);

            ApplicationUser updatedUser = _userUpdateService.UpdateTrialEmails(trial.TrialEmail, trialViewModel.TrialEmail);

            trial.TrialEmail = updatedUser;
            trial.TrialName = trialViewModel.TrialName;
            trial.IsActive = trialViewModel.IsActive;
            trial.TrialAttribution = trialViewModel.TrialAttribution;
            trial.TrialOwner = trialViewModel.TrialOwner;

            _trialRepository.Save();
        }
    }
}
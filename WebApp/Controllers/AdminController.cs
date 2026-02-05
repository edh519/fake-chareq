using BusinessLayer.Repositories.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.Services.Interfaces;
using BusinessLayer.ViewModels;
using DataAccessLayer.External.Repos;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Label = DataAccessLayer.Models.Label;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Authoriser")]
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ILabelRepository _labelRepository;
        private readonly ITrialRepository _trialRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly ITriageInfoRotaRepository _triageInfoRepository;
        private readonly GitHubService _gitHubService;
        private readonly ITrialManagementService _trialManagementService;
        private readonly IUserUpdateService _userUpdateService;

        public AdminController(ILabelRepository labelRepository, ITrialRepository trialRepository, ILogger<AdminController> logger, ITemplateRepository templateRepository, ITriageInfoRotaRepository triageInfoRepository, GitHubService gitHubService, ITrialManagementService trialManagementService, IUserUpdateService userUpdateService)
        {
            _labelRepository = labelRepository;
            _trialRepository = trialRepository;
            _logger = logger;
            _templateRepository = templateRepository;
            _triageInfoRepository = triageInfoRepository;
            _gitHubService = gitHubService;
            _trialManagementService = trialManagementService;
            _userUpdateService = userUpdateService;
        }
        
        #region Labels

        [HttpGet]
        public IActionResult ManageLabels(bool? isArchived = null)
        {
            LabelsViewModel manageLabelsViewModel = new();
            IEnumerable<Label> labels = _labelRepository.GetLabels();
            if (isArchived != null && labels != null && labels.Any())
                labels = labels.Where(x => x.IsArchived == isArchived);

            if (labels != null && labels.Any())
                manageLabelsViewModel = new(labels);

            return View(manageLabelsViewModel);
        }

        [HttpGet]
        public IActionResult AddLabel()
        {
            LabelViewModel labelViewModel = new();

            return View(labelViewModel);
        }

        [HttpPost]
        public IActionResult AddLabel(LabelViewModel labelViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(labelViewModel);
            }
            
            Label label = new()
            {
                LabelShort = labelViewModel.LabelShort,
                LabelDescription = labelViewModel.LabelDescription,
                HexColor = labelViewModel.HexColor
            };
            _ = _labelRepository.AddLabel(label);
            return RedirectToAction("ManageLabels", new { isArchived = false });
        }

        [HttpGet]
        public IActionResult EditLabel(int labelId)
        {
            if (labelId < 1)
            {
                return RedirectToAction("ManageLabels", new { isArchived = false });
            }

            Label? label = _labelRepository.GetLabel(labelId);
            if (label is null)
            {
                return RedirectToAction("ManageLabels", new { isArchived = false });
            }

            LabelViewModel labelViewModel = new(label);
            return View(labelViewModel);
        }

        [HttpPost]
        public IActionResult EditLabel(LabelViewModel labelViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(labelViewModel); 
            }
            
            Label label = _labelRepository.GetLabel(labelViewModel.LabelId);
            label.LabelShort = labelViewModel.LabelShort;
            label.LabelDescription = labelViewModel.LabelDescription;
            label.HexColor = labelViewModel.HexColor;
            label.IsArchived = labelViewModel.IsArchived;
            _labelRepository.Save();
            return RedirectToAction("ManageLabels", new { isArchived = false });
        }

        [HttpPut]
        public IActionResult ArchiveLabel(int labelId, bool archiveState)
        {
            if (labelId < 0)
            {
                string error = $"Invalid {nameof(labelId)}: {labelId}";
                _logger.LogError(error);
                return BadRequest(error);
            }

            Label? label = _labelRepository.GetLabel(labelId);
            if (label == null)
            {
                string error = $"Invalid {nameof(labelId)}: {labelId}";
                _logger.LogError(error);
                return BadRequest(error);
            }

            label.IsArchived = archiveState;
            _labelRepository.Save();
            return Ok(label.LabelId);
        }
        #endregion


        #region Trials

        [HttpGet]
        public IActionResult ManageTrials(bool? showActiveTrials)
        {
            TrialsViewModel manageTrialsViewModel = new();
            IEnumerable<Trial> trials = _trialRepository.Get();
            if (showActiveTrials != null && trials != null && trials.Any())
                trials = trials.Where(x => x.IsActive == showActiveTrials);

            if (trials != null && trials.Any())
                manageTrialsViewModel = new(trials);

            return View(manageTrialsViewModel);
        }

        [HttpGet]
        public IActionResult AddTrial(bool? showActiveTrials)
        {
            TrialViewModel trialViewModel = new();

            return View(trialViewModel);
        }

        [HttpPost]
        public IActionResult AddTrial(TrialViewModel trialViewModel, bool? showActiveTrials)
        {
            //Cannot use ModelState.IsValid as most of the viewModel is intentionaly blank, except Name.
            if (trialViewModel == null)
            {
                return View(trialViewModel);
            }

            if (string.IsNullOrEmpty(trialViewModel.TrialName))
            {
                return View(trialViewModel);
            }

            ApplicationUser? trialEmail = _userUpdateService.UpdateTrialEmails(null, trialViewModel.TrialEmail);

            Trial trial = new()
            {
                TrialName = trialViewModel.TrialName,
                TrialEmail = trialEmail,
                TrialAttribution = trialViewModel.TrialAttribution,
                TrialOwner = trialViewModel.TrialOwner,
            };
            _ = _trialRepository.AddTrial(trial);
            return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
        }

        [HttpGet]
        public async Task<IActionResult> EditTrial(int trialId, bool? showActiveTrials)
        {
            if (trialId < 1)
            {
                return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
            }
            
            if (trialId == 1000) // Trial "Other" cannot be edited.
            {
                _logger.LogInformation($"{nameof(EditTrial)}: Cannot edit Trial 'Other'.");
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Cannot edit Trial 'Other'."));
                return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
            }

            TrialViewModel? trialViewModel = await _trialManagementService.GetTrialViewModelAsync(trialId);
            if (trialViewModel == null)
            {
                return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
            }

            return View(trialViewModel);
        }

        [HttpPost]
        public IActionResult EditTrial(TrialViewModel trialViewModel, bool? showActiveTrials)
        {
            if (!ModelState.IsValid)
            {
                return View(trialViewModel);
            }
            
            if (trialViewModel.TrialId == 1000) // Trial "Other" cannot be edited. 
            {
                _logger.LogInformation($"{nameof(EditTrial)}: Cannot edit Trial 'Other'.");
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Cannot edit Trial 'Other'."));
                return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
            }

            _trialManagementService.UpdateTrialDetails(trialViewModel);
            return RedirectToAction(nameof(ManageTrials), new { showActiveTrials });
        }

        [HttpPost]
        public IActionResult DeleteRepository(int? repositoryId, int? trialId)
        {
            if (repositoryId is null || trialId is null)
            {
                return BadRequest();
            }
            bool success = _trialRepository.DeleteConfiguredRepository(trialId.Value, repositoryId.Value);

            if (success)
            {
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Repository deleted successfully."));
            }
            else
            {
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Repository could not be found."));
            }
            return RedirectToAction(nameof(EditTrial), new { trialId });
        }

        [HttpPost]
        public async Task<IActionResult> AddRepositoryToTrial(int? trialId, string repositoryId)
        {
            if (trialId is null || string.IsNullOrEmpty(repositoryId))
            {
                return BadRequest();
            }

            Repository repo = await _gitHubService.GetRepository(repositoryId);

            if (repo is null)
            {
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Repository does not exist."));
                return RedirectToAction(nameof(EditTrial), new { trialId });
            }
            Trial trial = _trialRepository.GetByID(trialId);

            if (trial == null)
            {
                TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: false, message: "Trial not found."));
                return RedirectToAction(nameof(ManageTrials));
            }

            trial.TrialRepositoryInfos.Add(new TrialRepositoryInfo { GitHubRepositoryId = repo.Id, TrialId = trialId.Value });
            _trialRepository.Save();

            TempData["PopupMessage"] = JsonConvert.SerializeObject(new PopupMessageViewModel(isSuccessful: true, message: "Repository added successfully."));
            return RedirectToAction(nameof(EditTrial), new { trialId });
        }


        [HttpPut]
        public IActionResult ArchiveTrial(int trialId, bool archiveState)
        {
            Trial trial = _trialRepository.GetByID(trialId);
            if (trial == null)
            {
                string error = $"Invalid {nameof(trialId)}: {trialId}";
                _logger.LogError(error);
                return BadRequest(error);
            }

            trial.IsActive = !archiveState;
            _trialRepository.Save();
            return Ok(trial.TrialId);
        }
        #endregion

        #region Information
        public IActionResult TriageInfo()
        {
            TriageInfoViewModel triageInfoViewModel = new TriageInfoViewModel();
            triageInfoViewModel.Rotas = _triageInfoRepository.GetAllRotas();
            triageInfoViewModel.REDCapTrials = _trialRepository.GetRedcapTrials();
            triageInfoViewModel.DevelopmentTrials = _trialRepository.GetDevelopmentTrials();
            triageInfoViewModel.NotAttributedTrials = _trialRepository.GetNotAttributedTrials();

            return View(triageInfoViewModel);
        }

        [HttpGet]
        public IActionResult EditRota(int rotaId)
        {
            TriageInfoRota rota = _triageInfoRepository.GetByID(rotaId);
            if (rota == null)
            {
                return RedirectToAction(nameof(TriageInfo));
            }
            TriageInfoRotaViewModel rotaViewModel = new TriageInfoRotaViewModel(rota);
            return View(rotaViewModel);
        }

        [HttpPost]
        public IActionResult EditRota(TriageInfoRotaViewModel triageInfoRotaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(triageInfoRotaViewModel);
            }
            
            TriageInfoRota rota = _triageInfoRepository.GetByID(triageInfoRotaViewModel.TriageInfoRotaId);
            rota.Day = triageInfoRotaViewModel.Day;
            rota.Morning = triageInfoRotaViewModel.Morning;
            rota.Afternoon = triageInfoRotaViewModel.Afternoon;
            rota.Reserve = triageInfoRotaViewModel.Reserve;
            _triageInfoRepository.Save();

            return RedirectToAction("TriageInfo");
        }
        #endregion
    }
}

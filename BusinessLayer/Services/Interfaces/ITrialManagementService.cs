using BusinessLayer.ViewModels;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interfaces;

public interface ITrialManagementService
{
    Task<TrialViewModel> GetTrialViewModelAsync(int trialId);
    void UpdateTrialDetails(TrialViewModel trialViewModel);
}
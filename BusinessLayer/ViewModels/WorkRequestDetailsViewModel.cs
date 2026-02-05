using DataAccessLayer.Models;
using Enums.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BusinessLayer.ViewModels
{

  public class WorkRequestDetailsViewModel
  {
    public WorkRequestDetailsViewModel(WorkRequest workRequest)
    {
      // Work Request fields
      WorkRequestId = workRequest.WorkRequestId;
      Reference = workRequest.Reference;
      Status = (int)workRequest.Status.WorkRequestStatusId;
      Trial = workRequest.Trial.TrialName;
      TrialOther = workRequest.TrialOther;
      TrialEmail = workRequest.Trial?.TrialEmail;
      CreationDateTime = workRequest.CreationDateTime;
      LastEditedDateTime = workRequest.LastEditedDateTime;
      DetailDescription = workRequest.DetailDescription;
      SupportingFiles = workRequest.SupportingFiles;
      CreatedBy = workRequest.CreatedBy;
      GitHubIssueNumber = workRequest.GitHubIssueNumber;

      SubTaskViewModel = new SubTaskViewModel();
      SubTaskViewModel.WorkRequestStatus = (int)workRequest.Status.WorkRequestStatusId;

      if (workRequest.Labels != null && workRequest.Labels.Any())
      {

        LabelsEditorViewModel = new()
        {
          LabelsAssigned = new(workRequest.Labels),
          WorkRequestId = WorkRequestId
        };
      }
      else
      {
        LabelsEditorViewModel = new()
        {
          LabelsAssigned = new(),
          WorkRequestId = WorkRequestId
        };
      }
    }
    public WorkRequestDetailsViewModel()
    {
      NewWorkRequestEvent = new();
    }



    public int WorkRequestId { get; set; }

    /// <summary>
    /// Is the current logged in user the owner/creator of the work request being shown - as they are able to edit their own requests.
    /// </summary>
    public bool IsLoggedInUserRequester { get; set; }
    public bool IsLoggedInUserOfRoleTypeUser { get; set; }

    public string MultipleAuthorisationsWarning { get; set; }
    public ProcessDeviationReason ProcessDeviationReason { get; set; }

    #region Relationships

    public IEnumerable<WorkRequestEventViewModel> WorkRequestEvents { get; set; }
    public IEnumerable<SubTaskEventViewModel> SubTaskEvents { get; set; }

    public IEnumerable<FileUpload> FileUploads { get; set; }

    public IEnumerable<Trial> Trials { get; set; }
    #endregion

    [Display(Name = "Labels")]
    public LabelEditorViewModel LabelsEditorViewModel { get; set; }
    public AssigneesViewModel AssigneesViewModel { get; set; }
    public SubTaskViewModel SubTaskViewModel { get; set; }
    public SubTaskAddViewModel SubTaskAddViewModel { get; set; }
    public SubscribeOtherUsersViewModel SubscribeOtherUsersViewModel { get; set; }

    public int Status { get; set; }

    [DisplayName("Title")]
    public string DisplayTitle => $"{Reference}";

    public string Reference { get; set; }

    [DisplayName("Trial Name")]
    public string Trial { get; set; }

    [DisplayName("Trial Other")]
    public string TrialOther { get; set; }
    [DisplayName("Trial Email")]
    public ApplicationUser? TrialEmail { get; set; }
    [DisplayName("Created on")]
    public DateTime CreationDateTime { get; set; }

    public DateTime LastEditedDateTime { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Detailed Description")]
    public string DetailDescription { get; set; }

    [DisplayName("Supporting Files")]
    public IEnumerable<FileUpload> SupportingFiles { get; set; }

    [DisplayName("Created by")]
    public string CreatedBy { get; set; }
    public int? GitHubIssueNumber { get; set; }
    public GitHubViewModel GitHubViewModel { get; set; }

    public IEnumerable<WorkRequestEventTypeEnum> AllowedEventTypes { get; set; }
    public WorkRequestEvent NewWorkRequestEvent { get; set; } = new();
    public SubTaskEvent NewSubTaskEvent { get; set; } = new();
    public bool ViewerIsSubscribed { get; set; }
    public bool TrialEmailIsSubscribed { get; set; }

    public List<ConversationViewModel> ConversationViewItems { get; set; }

    public List<IFormFile> NewSupportingFiles { get; set; }
  }
}

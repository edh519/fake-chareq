using BusinessLayer.Helpers;
using DataAccessLayer.Models;
using Enums.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.ViewModels
{
    public class ViewAllWorkRequestsViewModel
    {
        /// <summary>
        /// Constructor will apply the data bindings/mapping to the view model.
        /// Converts to user-readable and prettifies fields.
        /// </summary>
        /// <param name="workRequests"></param>
        public ViewAllWorkRequestsViewModel(IEnumerable<WorkRequest> workRequests)
        {
            List<WorkRequestViewModel> workRequestViewModels = new();

            if (workRequests != null)
            {
                foreach (WorkRequest workRequest in workRequests)
                {
                    workRequestViewModels.Add(new WorkRequestViewModel(workRequest));
                }
            }
            WorkRequests = workRequestViewModels;
        }




        public IEnumerable<WorkRequestViewModel> WorkRequests { get; set; }






        public class WorkRequestViewModel
        {

            public WorkRequestViewModel(WorkRequest workRequest)
            {
                Id = workRequest.WorkRequestId;
                Reference = workRequest.Reference;
                Status = workRequest.Status?.WorkRequestStatusName;
                Progress = CommonHelpers.ConvertWorkRequestStatusToProgress(workRequest.Status);
                Trial = workRequest.Trial?.TrialId == 1000 ? $"Other: {workRequest.TrialOther}" : workRequest.Trial?.TrialName;

                // Labels have to be converted into a simpler class as Label references itself for when JsonSerialised
                // i.e. Label > WorkRequest > Labels (Label) ...
                Labels = new List<LabelViewModel>();
                foreach (Label label in workRequest.Labels)
                {
                    Labels.Add(new LabelViewModel()
                    {
                        LabelId = label.LabelId,
                        LabelShort = label.LabelShort,
                        LabelDescription = label.LabelDescription,
                        HexColor = label.HexColor
                    });
                }

                // Assignees have to be converted into a simpler class as ApplicationUser references itself for when JsonSerialised
                // i.e. ApplicationUser > WorkRequest > Assignee (ApplicationUser) ...
                Assignees = new List<UserSimpleViewModel>();
                foreach (ApplicationUser assignee in workRequest.Assignees)
                {
                    Assignees.Add(new UserSimpleViewModel()
                    {
                        UserId = assignee.Id,
                        Username = assignee.UserName,
                        Email = assignee.Email
                    });
                }

                Requester = CommonHelpers.RemoveDomainFromEmail(workRequest.CreatedBy);
                CreatedDateTime = workRequest.CreationDateTime;

                // Fetch completed/closed dates depending on status.
                if (workRequest.Status.WorkRequestStatusId == WorkRequestStatusEnum.Completed)
                    CompletedDateTime = workRequest?.WorkRequestEvents.Where(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Complete).FirstOrDefault().CreatedAt;
                else if (workRequest.Status.WorkRequestStatusId == WorkRequestStatusEnum.Abandoned)
                    CompletedDateTime = workRequest?.WorkRequestEvents.Where(x => x.EventType.WorkRequestEventTypeId == WorkRequestEventTypeEnum.Closed).FirstOrDefault().CreatedAt;


                // HACK - Make Reference(title) to be the first bit of the description for legacy requests.
                if (Reference == null)
                {
                    if (workRequest.DetailDescription.Length > 40)
                        Reference = workRequest.DetailDescription.Take(40).ToString() + "...";
                    else
                        Reference = workRequest.DetailDescription;
                }
            }
            public int Id { get; set; }
            public string Reference { get; set; }
            public string Title => $"{Reference}";
            public string Status { get; set; }
            public double Progress { get; set; }
            public string Trial { get; set; }
            public string Impact { get; set; }
            public string Rationale { get; set; }
            public string Requester { get; set; }
            public List<LabelViewModel> Labels { get; set; }
            public List<UserSimpleViewModel> Assignees { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public DateTime? CompletedDateTime { get; set; }
        }



    }
}

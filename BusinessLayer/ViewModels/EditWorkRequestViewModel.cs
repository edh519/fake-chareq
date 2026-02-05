using BusinessLayer.Helpers;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace BusinessLayer.ViewModels
{
    public class EditWorkRequestViewModel
    {
        public EditWorkRequestViewModel(WorkRequest workRequest)
        {
            WorkRequestId = workRequest.WorkRequestId;
            Reference = workRequest.Reference;
            Status = (int)workRequest.Status.WorkRequestStatusId;
            Trial = workRequest.Trial.TrialId;
            TrialOther = workRequest.TrialOther;
            CreationDateTime = workRequest.CreationDateTime;
            DetailDescription = workRequest.DetailDescription;
            SupportingFiles = workRequest.SupportingFiles;
            CreatedBy = CommonHelpers.RemoveDomainFromEmail(workRequest.CreatedBy);
        }

        public EditWorkRequestViewModel()
        {

        }

        public int WorkRequestId { get; set; }

        #region Relationships

        public IEnumerable<FileUpload> FileUploads { get; set; }

        public IEnumerable<Trial> Trials { get; set; }
        #endregion

        [Required, DisplayName("Title"), MaxLength(50)]
        public string Reference { get; set; }

        public int Status { get; set; }

        [Required, DisplayName("Trial Name")]
        public int Trial { get; set; }

        [RequiredIf("Trial == 1000", ErrorMessage = "The Trial Other field is required."), DisplayName("Trial Other")]
        public string TrialOther { get; set; }

        [DisplayName("Created at")]
        public DateTime CreationDateTime { get; set; }

        [DataType(DataType.MultilineText)]
        [Required, DisplayName("Detailed Description")]
        public string DetailDescription { get; set; }

        [DisplayName("Upload More Supporting Files")]
        public IEnumerable<FileUpload> SupportingFiles { get; set; }

        [DisplayName("Created by")]
        public string CreatedBy { get; set; }

    }
}

using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using UoN.ExpressiveAnnotations.NetCore.Attributes;


namespace BusinessLayer.ViewModels
{
    public class WorkRequestViewModel
    {
        #region Lists for display

        public IEnumerable<Trial> Trials { get; set; }

        #endregion
        public List<int> SelectedTrialIds { get; set; } = new List<int>();


        public int WorkRequestId { get; set; }
        public string Title => $"{Reference}";
        [Required, DisplayName("Title"), MaxLength(50)]
        public string Reference { get; set; }

        [Required, DisplayName("Trial Name")]
        public int Trial { get; set; }

        [RequiredIf("Trial == 1000", ErrorMessage = "The Trial Other field is required."), DisplayName("Trial Other")]
        public string TrialOther { get; set; }

        [Required, DisplayName("Created at")]
        public DateTime CreationDateTime { get; set; }

        [Required, DataType(DataType.MultilineText), DisplayName("Detailed Description")]
        public string DetailDescription { get; set; }

        [DisplayName("Supporting Files")]
        public IEnumerable<IFormFile> SupportingFiles { get; set; }
    }
}

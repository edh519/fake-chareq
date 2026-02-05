using DataAccessLayer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
    public class LabelsViewModel
    {
        public LabelsViewModel() { }
        public LabelsViewModel(IEnumerable<Label> labels)
        {
            Labels = new();
            foreach (Label label in labels)
            {
                Labels.Add(new(label));
            }
        }
        public LabelsViewModel(IEnumerable<LabelViewModel> labels)
        {
            Labels = new();
            foreach (LabelViewModel label in labels)
            {
                Labels.Add(label);
            }
        }

        public List<LabelViewModel> Labels { get; set; }
    }

    public class LabelViewModel
    {
        public LabelViewModel() { }
        public LabelViewModel(Label label)
        {
            LabelId = label.LabelId;
            LabelShort = label.LabelShort;
            LabelDescription = label.LabelDescription;
            HexColor = label.HexColor;
            IsArchived = label.IsArchived;
        }

        public int LabelId { get; set; }
        [Display(Name = "Label"), Required, MaxLength(25)]
        public string LabelShort { get; set; }
        [Display(Name = "Description"), Required, MaxLength(50)]
        public string LabelDescription { get; set; }
        [Display(Name = "Colour"), Required]
        public string HexColor { get; set; }
        public bool IsArchived { get; set; } = false;

        public int? WorkRequestId { get; set; } = null;
    }
}

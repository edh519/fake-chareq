using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLayer.ViewModels;

public class DataExportViewModel
{
    [DisplayName("Trial")]
    public int SelectedTrialId { get; set; }
    public List<SelectListItem> Trials { get; set; } = [];
    public List<DataExportJobViewModel> ExportHistory { get; set; } = [];
}
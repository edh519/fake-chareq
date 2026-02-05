using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums.Enums;

public enum TrialAttribution
{
    [Display(Name = "REDCap", Description = "ytu-redcap-group@york.ac.uk")]
    REDCap,

    [Display(Name = "Development", Description = "ytu-developers-group@york.ac.uk")]
    Development
}
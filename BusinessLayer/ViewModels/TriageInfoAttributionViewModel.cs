using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class TriageInfoAttributionViewModel
{
    List<Trial> REDCapTrials {  get; set; }
    List<Trial> DevelopmentTrials { get; set; }
    List<Trial> NotAttributedTrials { get; set; }
}

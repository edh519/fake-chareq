using System;
using System.Collections.Generic;
using DataAccessLayer.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class TriageInfoViewModel
{
    public List<TriageInfoRota> Rotas { get; set; }
    public List<Trial> REDCapTrials { get; set; }
    public List<Trial> DevelopmentTrials { get; set; }
    public List<Trial> NotAttributedTrials { get; set; }
}

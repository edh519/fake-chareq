using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class StaleWorkRequestEmailViewModel
{
    public List<StaleWorkRequest> StaleWorkRequests {  get; set; }
    public string SystemUrl { get; set; }
    public string ViewWorkRequestUrl { get; set; }
}

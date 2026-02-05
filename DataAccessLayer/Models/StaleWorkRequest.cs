using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

public class StaleWorkRequest
{
    public List<string> InvolvedUsers { get; set; }
    public WorkRequest WorkRequest { get; set; }
}

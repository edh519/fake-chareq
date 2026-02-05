using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enums;
using Enums.Enums;

namespace DataAccessLayer.Models;

public class TriageInfoRota
{
    [Required]
    public int TriageInfoRotaId { get; set; }
    [Required]
    public WeekdayEnum Day { get; set; }
    public string Morning { get; set; }
    public string Afternoon { get; set; }
    public string Reserve { get; set; }
}

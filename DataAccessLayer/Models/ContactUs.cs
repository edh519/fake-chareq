using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models;

public class ContactUs
{
    public int ContactUsId { get; set; }

    #region Contact Us Form Details

    [Required, Column(TypeName = "varchar(max)")]
    public string Message { get; set; }
    [Required, EmailAddress, Column(TypeName = "varchar(255)")]
    public string Email { get; set; }
    public DateTime Submitted { get; set; }

    #endregion

    [Column(TypeName = "varchar(255)")]
    public string? UpdatedBy { get; set; }


    #region Management Functionality

    [Column(TypeName = "varchar(max)")] public string? Notes { get; set; }
    public bool Actioned { get; set; }
    [Column(TypeName = "varchar(255)")] public string? ActionedBy { get; set; }
    public DateTime? ActionedAt { get; set; }

    #endregion
}

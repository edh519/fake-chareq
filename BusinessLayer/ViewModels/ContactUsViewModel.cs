using BusinessLayer.Helpers;
using DataAccessLayer.Validators;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ViewModels;

public class ContactUsViewModel
{
    public int Id { get; set; }
    public string Email { get; set; }
    [Required]
    public string Message { get; set; }
    [DisplayName("Submitted At"), DataType(DataType.DateTime)]
    public DateTime Submitted { get; set; }
    public string? Notes { get; set; }
    public bool Actioned { get; set; }
    public string? ActionedBy { get; set; }
    [DisplayName("Date Actioned"), DataType(DataType.Date)]
    [DateLessThanOrEqualToToday]
    public DateTime? ActionedAt { get; set; }
    public object Name { get; set; }

    public string GetDisplayMessage()
    {
        var safeHtml = HtmlHelper.RemoveImageElementsFromString(Message, false);

        if (string.IsNullOrEmpty(safeHtml))
            return string.Empty;
        return safeHtml.Length > 100 ? safeHtml.Substring(0, 100) + "..." : safeHtml;
    }
}

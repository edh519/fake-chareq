using BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
    public class YTULibraryUserViewModel 
    {

        [Display(Name = "Login Type")]
        public string LoginType { get; set; }
        public List<SelectListItem> LoginTypesSelectList { get; set; }


        [Display(Name = "Lock Type")]
        public string LockType { get; set; }
        public List<SelectListItem> LockTypesSelectList { get; set; }


        [Display(Name = "Admin Role")]
        public string AdminRole { get; set; }
        public List<SelectListItem> AdminRolesSelectList { get; set; }


        

       

    }
}

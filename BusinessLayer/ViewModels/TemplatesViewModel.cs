using DataAccessLayer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.ViewModels
{
    public class TemplatesViewModel
    {
        public TemplatesViewModel() { }
        public TemplatesViewModel(IEnumerable<Template> templates)
        {
            Templates = new();
            foreach (Template template in templates)
            {
                Templates.Add(new(template));
            }
        }
        public TemplatesViewModel(IEnumerable<TemplateViewModel> templates)
        {
            Templates = new();
            foreach (TemplateViewModel template in templates)
            {
                Templates.Add(template);
            }
        }

        public List<TemplateViewModel> Templates { get; set; }

    }

    public class TemplateViewModel
    {
        public TemplateViewModel() { }
        public TemplateViewModel(Template Template)
        {
            TemplateId = Template.TemplateId;
            TemplateShort = Template.TemplateShort;
            TemplateDescription = Template.TemplateDescription;
            TemplateLayout = Template.TemplateLayout;
            IsArchived = Template.IsArchived;
        }

        public int TemplateId { get; set; }
        [Display(Name = "Template"), Required, MaxLength(25)]
        public string TemplateShort { get; set; }
        [Display(Name = "Description"), Required, MaxLength(250)]
        public string TemplateDescription { get; set; }
        [Display(Name = "Layout"), Required]
        public string TemplateLayout { get; set; }
        public bool IsArchived { get; set; } = false;

    }

}

using DataAccessLayer.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessLayer.ViewModels
{
    public class TemplatesTinyMCEViewModel
    {
        public TemplatesTinyMCEViewModel() { }
        public TemplatesTinyMCEViewModel(IEnumerable<Template> templates)
        {
            Templates = new();
            foreach (Template template in templates)
            {
                Templates.Add(new(template));
            }
        }

        public TemplatesTinyMCEViewModel(IEnumerable<TemplateViewModel> templates)
        {
            Templates = new();
            foreach (TemplateViewModel template in templates)
            {
                Templates.Add(new(template));
            }
        }

        public List<TemplateTinyMCEViewModel> Templates { get; set; }
    }

    public class TemplateTinyMCEViewModel
    {
        public TemplateTinyMCEViewModel() { }
        public TemplateTinyMCEViewModel(Template Template)
        {
            Title = Template.TemplateShort;
            Description = Template.TemplateDescription;
            Content = Template.TemplateLayout;
        }

        public TemplateTinyMCEViewModel(TemplateViewModel Template)
        {
            Title = Template.TemplateShort;
            Description = Template.TemplateDescription;
            Content = Template.TemplateLayout;
        }

        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }

    }
}

namespace DataAccessLayer.Models
{
    public class Template
    {
        public int TemplateId { get; set; }
        /// <summary>
        /// Name of the template as it appears on screen.
        /// </summary>
        public string TemplateShort { get; set; }
        /// <summary>
        /// Description of when this template should be used.
        /// </summary>
        public string TemplateDescription { get; set; }
        /// <summary>
        /// The html markup of the template when displayed.
        /// </summary>
        public string TemplateLayout { get; set; }
        /// <summary>
        /// Determines whether this template is a selectable template to add.
        /// Can exist on old requests when archived.
        /// </summary>
        public bool IsArchived { get; set; }

    }
}
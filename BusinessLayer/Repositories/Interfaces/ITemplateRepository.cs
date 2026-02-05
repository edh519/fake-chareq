using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface ITemplateRepository : IRepository<Template>
    {
        public Template GetTemplate(int labelId);
        public int AddTemplate(Template label);
        public int ArchiveTemplate(int labelId);
        public int UnarchiveTemplate(int labelId);
        public IEnumerable<Template> GetTemplates();
        public IEnumerable<Template> GetUnarchivedTemplates();

    }
}

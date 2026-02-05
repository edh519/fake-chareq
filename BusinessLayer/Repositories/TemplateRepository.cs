using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class TemplateRepository : Repository<Template>, ITemplateRepository
    {
        public TemplateRepository(ApplicationDbContext context) : base(context)
        { }

        public Template GetTemplate(int TemplateId)
        {
            return _context.Templates
                .Where(x => x.TemplateId == TemplateId)
                .SingleOrDefault();
        }


        public int AddTemplate(Template Template)
        {
            _context.Add(Template);
            _context.SaveChanges();
            return Template.TemplateId;
        }

        public int ArchiveTemplate(int TemplateId)
        {
            Template Template = GetTemplate(TemplateId);
            Template.IsArchived = true;
            _context.SaveChanges();
            return TemplateId;
        }

        public IEnumerable<Template> GetTemplates()
        {
            return _context.Templates;
        }

        public IEnumerable<Template> GetUnarchivedTemplates()
        {
            return _context.Templates.Where(x => !x.IsArchived);
        }

        public int UnarchiveTemplate(int TemplateId)
        {
            Template Template = GetTemplate(TemplateId);
            Template.IsArchived = false;
            _context.SaveChanges();
            return TemplateId;
        }
    }
}

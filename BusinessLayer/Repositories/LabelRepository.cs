using BusinessLayer.Repositories.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Repositories
{
    public class LabelRepository : Repository<Label>, ILabelRepository
    {
        public LabelRepository(ApplicationDbContext context) : base(context)
        { }

        public Label? GetLabel(int labelId)
        {
            return _context.Label.Find(labelId);
        }


        public int AddLabel(Label label)
        {
            _context.Add(label);
            _context.SaveChanges();
            return label.LabelId;
        }

        public int ArchiveLabel(int labelId)
        {
            Label label = GetLabel(labelId);
            label.IsArchived = true;
            _context.SaveChanges();
            return labelId;
        }

        public IEnumerable<Label> GetLabels()
        {
            return _context.Label;
        }

        public IEnumerable<Label> GetUnarchivedLabels()
        {
            return _context.Label.Where(x => !x.IsArchived);
        }

        public int UnarchiveLabel(int labelId)
        {
            Label label = GetLabel(labelId);
            label.IsArchived = false;
            _context.SaveChanges();
            return labelId;
        }
    }
}

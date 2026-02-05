using DataAccessLayer.Models;
using System.Collections.Generic;

namespace BusinessLayer.Repositories.Interfaces
{
    public interface ILabelRepository : IRepository<Label>
    {
        public Label GetLabel(int labelId);
        public int AddLabel(Label label);
        public int ArchiveLabel(int labelId);
        public int UnarchiveLabel(int labelId);
        public IEnumerable<Label> GetLabels();
        public IEnumerable<Label> GetUnarchivedLabels();



    }
}

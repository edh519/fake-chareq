using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class ProcessDeviationReason
    {

        public int ProcessDeviationReasonId { get; set; }
        [Column(TypeName = "varchar(MAX)"), Required]
        public string Reason { get; set; }

    }
}

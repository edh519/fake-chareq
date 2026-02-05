using System.Collections.Generic;

namespace DataAccessLayer.Models
{
    public class Label
    {
        public int LabelId { get; set; }
        /// <summary>
        /// Name of the label as it appears on screen.
        /// </summary>
        public string LabelShort { get; set; }
        /// <summary>
        /// Description of when this label should be used.
        /// </summary>
        public string LabelDescription { get; set; }
        /// <summary>
        /// The hex color of the label when displayed.
        /// </summary>
        public string HexColor { get; set; }
        /// <summary>
        /// Determines whether this label is a selectable label to add.
        /// Can exist on old requests when archived.
        /// </summary>
        public bool IsArchived { get; set; }

        public IEnumerable<WorkRequest> WorkRequests { get; set; }

    }
}

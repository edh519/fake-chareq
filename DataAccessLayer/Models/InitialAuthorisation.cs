using System;

namespace DataAccessLayer.Models
{
    public class InitialAuthorisation
    {
        public int InitialAuthorisationId { get; set; }
        /// <summary>
        /// Estimated time impact in days
        /// e.g. 0.5 days, 1 day
        /// </summary>
        public double EstimatedTimeImpact { get; set; }
        public string WorkRequiredDecription { get; set; }
        public string DecisionBy { get; set; }
        public DateTime DecisionDateTime { get; set; }
    }
}

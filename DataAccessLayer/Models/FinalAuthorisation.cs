using System;

namespace DataAccessLayer.Models
{
    public class FinalAuthorisation
    {
        public int FinalAuthorisationId { get; set; }
        public string WorkReference { get; set; }
        public double ActualTimeImpactDays { get; set; }
        public string CompletedBy { get; set; }
        public DateTime CompletedDateTime { get; set; }
        public ProcessDeviationReason ProcessDeviationReason { get; set; }
        public int? ProcessDeviationReasonId { get; set; }
        public string Comments { get; set; }
    }
}

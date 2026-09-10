using System;
using System.Collections.Generic;

namespace MilestoneManagementClient.Models
{
    public enum IncidentSeverity
    {
        Critical,
        High,
        Medium,
        Low,
        Informational
    }

    public enum IncidentStatus
    {
        Open,
        UnderInvestigation,
        EvidenceSecured,
        Closed
    }

    public class IncidentModel
    {
        public Guid Id { get; set; }
        public string IncidentNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IncidentSeverity Severity { get; set; }
        public IncidentStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string PrimaryCameraName { get; set; }
        public string CreatedBy { get; set; }
        public bool IsEvidenceLocked { get; set; }
        public DateTime EvidenceRetentionUntil { get; set; }
        public List<string> BookmarkedSequences { get; set; }

        public IncidentModel()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
            Severity = IncidentSeverity.Medium;
            Status = IncidentStatus.Open;
            IsEvidenceLocked = true;
            EvidenceRetentionUntil = DateTime.Now.AddYears(5);
            BookmarkedSequences = new List<string>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core.IncidentManager
{
    public class IncidentManagerService
    {
        private readonly List<IncidentModel> _incidents = new List<IncidentModel>();

        public IncidentManagerService()
        {
            InitializeSampleIncidents();
        }

        private void InitializeSampleIncidents()
        {
            _incidents.Add(new IncidentModel
            {
                IncidentNumber = "INC-2025-00104",
                Title = "Perimeter Intrusion Detected at North Logistics Fence",
                Description = "Thermal AI tripwire triggered along north perimeter boundary after hours.",
                Severity = IncidentSeverity.High,
                Status = IncidentStatus.EvidenceSecured,
                PrimaryCameraName = "FLIR-SAROS-THERMAL-01",
                CreatedBy = "AI Perimeter Analytics Agent",
                IsEvidenceLocked = true,
                BookmarkedSequences = new List<string> { "Clip_20250831_021500.mp4 (Locked)" }
            });

            _incidents.Add(new IncidentModel
            {
                IncidentNumber = "INC-2025-00105",
                Title = "Unrecognized Vehicle Entry at Gate #3",
                Description = "LPR Camera captured unlisted license plate entering through loading bay barrier.",
                Severity = IncidentSeverity.Medium,
                Status = IncidentStatus.UnderInvestigation,
                PrimaryCameraName = "AXIS-PERIMETER-P1455-01",
                CreatedBy = "LPR Operator Node",
                IsEvidenceLocked = true,
                BookmarkedSequences = new List<string> { "LPR_Clip_Gate3_PlateMatch.mp4" }
            });

            _incidents.Add(new IncidentModel
            {
                IncidentNumber = "INC-2025-00106",
                Title = "Server Room Door Forced Open Alarm",
                Description = "Contact sensor and reader mismatch at secure data center portal.",
                Severity = IncidentSeverity.Critical,
                Status = IncidentStatus.Open,
                PrimaryCameraName = "HIKVISION-COLORVU-4K-01",
                CreatedBy = "PACS Integration Bridge",
                IsEvidenceLocked = true,
                BookmarkedSequences = new List<string> { "PACS_ServerRoom_Forced_0831.mp4" }
            });
        }

        public List<IncidentModel> GetAllIncidents() { return _incidents; }

        public void CreateIncident(string title, string desc, IncidentSeverity severity, string camera, string creator)
        {
            var inc = new IncidentModel
            {
                IncidentNumber = "INC-2025-00" + (_incidents.Count + 107),
                Title = title,
                Description = desc,
                Severity = severity,
                PrimaryCameraName = camera,
                CreatedBy = creator,
                Status = IncidentStatus.Open,
                IsEvidenceLocked = true
            };
            _incidents.Insert(0, inc);
        }

        public void UpdateStatus(Guid id, IncidentStatus status)
        {
            var inc = _incidents.FirstOrDefault(i => i.Id == id);
            if (inc != null) inc.Status = status;
        }
    }
}

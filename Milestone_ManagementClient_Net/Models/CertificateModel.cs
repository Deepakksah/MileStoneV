using System;

namespace MilestoneManagementClient.Models
{
    public class CertificateModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Thumbprint { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string ServiceBinding { get; set; } // Management Server, Recording Server, Mobile Gateway
        public bool IsBound { get; set; }
        public bool HasPrivateKey { get; set; }

        public CertificateModel()
        {
            Id = Guid.NewGuid();
            ValidFrom = DateTime.Now.AddDays(-30);
            ValidTo = DateTime.Now.AddYears(2);
            HasPrivateKey = true;
        }
    }

    public enum FailoverMode
    {
        ColdStandby,
        HotStandby,
        ActiveActive
    }

    public class FailoverServerModel
    {
        public Guid Id { get; set; }
        public string ServerName { get; set; }
        public string HostIp { get; set; }
        public FailoverMode Mode { get; set; }
        public string AssignedPrimaryServer { get; set; }
        public bool IsActiveTakingOver { get; set; }
        public int HeartbeatIntervalMs { get; set; }
        public DateTime LastHeartbeat { get; set; }

        public FailoverServerModel()
        {
            Id = Guid.NewGuid();
            Mode = FailoverMode.HotStandby;
            HeartbeatIntervalMs = 2000;
            LastHeartbeat = DateTime.Now;
        }
    }
}

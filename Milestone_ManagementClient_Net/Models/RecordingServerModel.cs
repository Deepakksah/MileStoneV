using System;

namespace MilestoneManagementClient.Models
{
    public class RecordingServerModel
    {
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Status { get; set; }
        public int DeviceCount { get; set; }
        public int CpuUsage { get; set; }
        public int RamUsage { get; set; }
        public string PrimaryStoragePath { get; set; }
        public double StorageUsedGb { get; set; }
        public double StorageTotalGb { get; set; }
        public string ArchiveStoragePath { get; set; }
        public double ArchiveUsedGb { get; set; }
        public double ArchiveTotalGb { get; set; }
        public bool IsFailoverConfigured { get; set; }
        public string FailoverServerName { get; set; }

        public RecordingServerModel()
        {
            ServerId = Guid.NewGuid().ToString();
            Name = "REC-SRV-01 (Primary)";
            Hostname = "192.168.1.101";
            Port = 7563;
            Status = "Running (Active)";
            DeviceCount = 12;
            CpuUsage = 28;
            RamUsage = 42;
            PrimaryStoragePath = @"D:\Milestone\MediaDatabase\Live";
            StorageUsedGb = 3420;
            StorageTotalGb = 8000;
            ArchiveStoragePath = @"E:\Milestone\Archive";
            ArchiveUsedGb = 14500;
            ArchiveTotalGb = 32000;
            IsFailoverConfigured = true;
            FailoverServerName = "FAILOVER-SRV-01 (Hot Standby)";
        }
    }
}

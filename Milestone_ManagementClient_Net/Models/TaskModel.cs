using System;

namespace MilestoneManagementClient.Models
{
    public class TaskModel
    {
        public string TaskId { get; set; }
        public string Name { get; set; }
        public string TargetServer { get; set; }
        public int ProgressPercent { get; set; }
        public string Status { get; set; } // "Running", "Completed", "Queued", "Error"
        public DateTime StartedAt { get; set; }

        public TaskModel()
        {
            TaskId = Guid.NewGuid().ToString().Substring(0, 8);
            Name = "Storage Archiving & Grooming";
            TargetServer = "REC-SRV-01";
            ProgressPercent = 74;
            Status = "Running";
            StartedAt = DateTime.Now.AddMinutes(-15);
        }

        public TaskModel(string name, string srv, int progress, string status)
        {
            TaskId = Guid.NewGuid().ToString().Substring(0, 8);
            Name = name;
            TargetServer = srv;
            ProgressPercent = progress;
            Status = status;
            StartedAt = DateTime.Now.AddMinutes(-5);
        }
    }

    public class LogEntryModel
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } // "INFO", "WARN", "ERROR"
        public string Source { get; set; }
        public string Message { get; set; }
        public string User { get; set; }

        public LogEntryModel()
        {
            Timestamp = DateTime.Now;
            Level = "INFO";
            Source = "Management Server";
            Message = "Service started successfully";
            User = "SYSTEM";
        }

        public LogEntryModel(string level, string source, string msg, string user)
        {
            Timestamp = DateTime.Now;
            Level = level;
            Source = source;
            Message = msg;
            User = user;
        }
    }
}

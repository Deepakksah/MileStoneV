using System;

namespace MilestoneManagementClient.Models
{
    public class RuleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string EventTrigger { get; set; }
        public string TargetSource { get; set; }
        public string TimeProfile { get; set; }
        public string Action { get; set; }
        public string LogEntry { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDefaultRule { get; set; }

        public RuleModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = "New Rule";
            Description = "";
            EventTrigger = "Motion Started (HW)";
            TargetSource = "Camera Group 1";
            TimeProfile = "Always";
            Action = "Start recording 3 seconds before event";
            LogEntry = "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$";
            IsEnabled = true;
            IsDefaultRule = false;
        }

        public RuleModel(string name, string trigger, string target, string timeProfile, string action, bool enabled, string description = "", string logEntry = "$DeviceName$$Event Name$$TriggerTime$$RuleName$$RecorderName$", bool isDefault = false)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            EventTrigger = trigger;
            TargetSource = target;
            TimeProfile = timeProfile;
            Action = action;
            LogEntry = logEntry;
            IsEnabled = enabled;
            IsDefaultRule = isDefault;
        }
    }
}

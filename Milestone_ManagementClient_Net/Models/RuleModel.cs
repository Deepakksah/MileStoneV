using System;

namespace MilestoneManagementClient.Models
{
    public class RuleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EventTrigger { get; set; }
        public string TargetSource { get; set; }
        public string TimeProfile { get; set; }
        public string Action { get; set; }
        public bool IsEnabled { get; set; }

        public RuleModel()
        {
            Id = Guid.NewGuid().ToString();
            Name = "Rule 01: Motion -> Start Recording";
            EventTrigger = "Motion Started";
            TargetSource = "All Cameras in South Block";
            TimeProfile = "Always";
            Action = "Start recording on related device at 25 fps";
            IsEnabled = true;
        }

        public RuleModel(string name, string trigger, string target, string timeProfile, string action, bool enabled)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            EventTrigger = trigger;
            TargetSource = target;
            TimeProfile = timeProfile;
            Action = action;
            IsEnabled = enabled;
        }
    }
}

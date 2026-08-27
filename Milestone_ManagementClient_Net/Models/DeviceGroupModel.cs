using System.Collections.Generic;

namespace MilestoneManagementClient.Models
{
    public class DeviceGroupModel
    {
        public string GroupName { get; set; }
        public bool IsExpanded { get; set; }
        public List<CameraModel> Cameras { get; set; }

        public DeviceGroupModel()
        {
            GroupName = "";
            IsExpanded = true;
            Cameras = new List<CameraModel>();
        }
    }
}

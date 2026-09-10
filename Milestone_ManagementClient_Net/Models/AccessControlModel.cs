using System;
using System.Collections.Generic;

namespace MilestoneManagementClient.Models
{
    public enum DoorState
    {
        Locked,
        Unlocked,
        Open,
        ForcedOpen,
        HeldOpen,
        Disabled
    }

    public class AccessDoorModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AssociatedCameraName { get; set; }
        public DoorState State { get; set; }
        public string ReaderType { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastAccessTime { get; set; }
        public string LastCardHolder { get; set; }

        public AccessDoorModel()
        {
            Id = Guid.NewGuid();
            State = DoorState.Locked;
            IsOnline = true;
            LastAccessTime = DateTime.Now;
        }
    }

    public class CardholderModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string BadgeNumber { get; set; }
        public string Department { get; set; }
        public string AccessLevel { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpirationDate { get; set; }

        public CardholderModel()
        {
            Id = Guid.NewGuid();
            IsActive = true;
            ExpirationDate = DateTime.Now.AddYears(1);
        }
    }

    public class AccessEventModel
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string DoorName { get; set; }
        public string CardholderName { get; set; }
        public string BadgeNumber { get; set; }
        public bool Granted { get; set; }
        public string Reason { get; set; }

        public AccessEventModel()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
    }
}

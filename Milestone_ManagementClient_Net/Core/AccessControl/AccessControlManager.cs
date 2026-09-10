using System;
using System.Collections.Generic;
using System.Linq;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core.AccessControl
{
    public class AccessControlManager
    {
        private readonly List<AccessDoorModel> _doors = new List<AccessDoorModel>();
        private readonly List<CardholderModel> _cardholders = new List<CardholderModel>();
        private readonly List<AccessEventModel> _events = new List<AccessEventModel>();

        public AccessControlManager()
        {
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            _doors.Add(new AccessDoorModel
            {
                Name = "Main Reception SpeedGate A1",
                Location = "Building 1 - Ground Floor",
                AssociatedCameraName = "LYCHEE MAIN ENTRANCE CAM 01",
                State = DoorState.Locked,
                ReaderType = "HID Signo RFID + Mobile BLE",
                LastCardHolder = "Deepak Sah (Lead Engineer)",
                LastAccessTime = DateTime.Now.AddMinutes(-4)
            });

            _doors.Add(new AccessDoorModel
            {
                Name = "Server Room Data Center Portal",
                Location = "Building 1 - Level 2 Secure Bay",
                AssociatedCameraName = "HIKVISION-COLORVU-4K-01",
                State = DoorState.Locked,
                ReaderType = "BioMini Biometric Fingerprint + SmartCard",
                LastCardHolder = "Vikram Patel (Systems Architect)",
                LastAccessTime = DateTime.Now.AddMinutes(-12)
            });

            _doors.Add(new AccessDoorModel
            {
                Name = "Perimeter Loading Bay Barrier Gate",
                Location = "North Logistics Compound",
                AssociatedCameraName = "AXIS-PERIMETER-P1455-01",
                State = DoorState.Locked,
                ReaderType = "Nedap ANPR LPR + Long-Range RFID UHF",
                LastCardHolder = "Logistics Vehicle #DL-01-AX-9902",
                LastAccessTime = DateTime.Now.AddMinutes(-25)
            });

            _doors.Add(new AccessDoorModel
            {
                Name = "Executive Boardroom Dual-Leaf Door",
                Location = "Building 1 - Level 4 Penthouse",
                AssociatedCameraName = "DAHUA-WIZMIND-4K-01",
                State = DoorState.Locked,
                ReaderType = "Mifare DESFire EV3 Contactless",
                LastCardHolder = "Executive Director",
                LastAccessTime = DateTime.Now.AddHours(-1)
            });

            _cardholders.Add(new CardholderModel { FullName = "Deepak Sah", BadgeNumber = "IC-88091", Department = "VMS Platform Engineering", AccessLevel = "Full Unrestricted 24/7 Superuser" });
            _cardholders.Add(new CardholderModel { FullName = "Vikram Patel", BadgeNumber = "IC-77021", Department = "IT Infrastructure & Security", AccessLevel = "Server Rooms & Data Centers" });
            _cardholders.Add(new CardholderModel { FullName = "Ananya Sen", BadgeNumber = "IC-66014", Department = "Surveillance Operations", AccessLevel = "Control Room & Operations Floor" });
            _cardholders.Add(new CardholderModel { FullName = "Rajesh Sharma", BadgeNumber = "IC-55099", Department = "Facilities & Logistics", AccessLevel = "Perimeter Gates & Loading Bays" });

            LogAccessEvent("Main Reception SpeedGate A1", "Deepak Sah", "IC-88091", true, "Access Granted (Valid Badge & Multi-Factor)");
            LogAccessEvent("Server Room Data Center Portal", "Vikram Patel", "IC-77021", true, "Access Granted (Biometric Matched)");
            LogAccessEvent("Executive Boardroom Dual-Leaf Door", "Unknown Badge", "BADGE-ERR-99", false, "Access Denied (Invalid Cardholder)");
        }

        public List<AccessDoorModel> GetDoors() { return _doors; }
        public List<CardholderModel> GetCardholders() { return _cardholders; }
        public List<AccessEventModel> GetRecentEvents() { return _events; }

        public void UnlockDoor(Guid doorId, int durationSeconds = 5)
        {
            var door = _doors.FirstOrDefault(d => d.Id == doorId);
            if (door != null)
            {
                door.State = DoorState.Unlocked;
                LogAccessEvent(door.Name, "Operator Console", "CONSOLE-OP", true, "Remote Pulse Unlock (" + durationSeconds + "s)");
            }
        }

        public void LockDoor(Guid doorId)
        {
            var door = _doors.FirstOrDefault(d => d.Id == doorId);
            if (door != null)
            {
                door.State = DoorState.Locked;
                LogAccessEvent(door.Name, "Operator Console", "CONSOLE-OP", true, "Remote Lock Enforced");
            }
        }

        public void LogAccessEvent(string doorName, string cardholder, string badge, bool granted, string reason)
        {
            _events.Insert(0, new AccessEventModel
            {
                DoorName = doorName,
                CardholderName = cardholder,
                BadgeNumber = badge,
                Granted = granted,
                Reason = reason,
                Timestamp = DateTime.Now
            });

            if (_events.Count > 100) _events.RemoveAt(_events.Count - 1);
        }
    }
}

using System;
using System.Collections.Generic;
using MilestoneManagementClient.Models;

namespace MilestoneManagementClient.Core.SmartMap
{
    public class SmartMapManager
    {
        private readonly List<MapPinModel> _pins = new List<MapPinModel>();

        public SmartMapManager()
        {
            InitializeSamplePins();
        }

        private void InitializeSamplePins()
        {
            _pins.Add(new MapPinModel
            {
                Name = "LYCHEE MAIN ENTRANCE CAM 01",
                DeviceType = "PTZ",
                PixelX = 160,
                PixelY = 180,
                DirectionFovAngle = 45f,
                FovSpan = 90f,
                IsOnline = true,
                HasActiveAlarm = false
            });

            _pins.Add(new MapPinModel
            {
                Name = "HIKVISION-COLORVU-4K-01",
                DeviceType = "Camera",
                PixelX = 340,
                PixelY = 140,
                DirectionFovAngle = 135f,
                FovSpan = 80f,
                IsOnline = true,
                HasActiveAlarm = true
            });

            _pins.Add(new MapPinModel
            {
                Name = "AXIS-PERIMETER-P1455-01",
                DeviceType = "Camera",
                PixelX = 520,
                PixelY = 260,
                DirectionFovAngle = 220f,
                FovSpan = 70f,
                IsOnline = true,
                HasActiveAlarm = false
            });

            _pins.Add(new MapPinModel
            {
                Name = "FLIR-SAROS-THERMAL-01",
                DeviceType = "Thermal",
                PixelX = 420,
                PixelY = 360,
                DirectionFovAngle = 315f,
                FovSpan = 60f,
                IsOnline = true,
                HasActiveAlarm = true
            });

            _pins.Add(new MapPinModel
            {
                Name = "Main Reception SpeedGate A1",
                DeviceType = "Door",
                PixelX = 220,
                PixelY = 220,
                IsOnline = true,
                HasActiveAlarm = false
            });
        }

        public List<MapPinModel> GetPins() { return _pins; }
    }
}

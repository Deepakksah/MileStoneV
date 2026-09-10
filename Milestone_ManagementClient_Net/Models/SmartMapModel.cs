using System;
using System.Collections.Generic;

namespace MilestoneManagementClient.Models
{
    public class MapPinModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DeviceType { get; set; } // Camera, PTZ, Door, Alarm, Server
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PixelX { get; set; }
        public int PixelY { get; set; }
        public float DirectionFovAngle { get; set; }
        public float FovSpan { get; set; }
        public bool IsOnline { get; set; }
        public bool HasActiveAlarm { get; set; }

        public MapPinModel()
        {
            Id = Guid.NewGuid();
            IsOnline = true;
            FovSpan = 75f;
        }
    }

    public class MapLayerModel
    {
        public Guid Id { get; set; }
        public string LayerName { get; set; }
        public string Provider { get; set; } // OpenStreetMap, Google Satellite, CAD Floorplan
        public int ZoomLevel { get; set; }
        public List<MapPinModel> Pins { get; set; }

        public MapLayerModel()
        {
            Id = Guid.NewGuid();
            Pins = new List<MapPinModel>();
        }
    }
}

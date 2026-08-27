using System;

namespace MilestoneManagementClient.Models
{
    public class CameraModel
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public string Resolution { get; set; }
        public string Codec { get; set; }
        public int Fps { get; set; }
        public string BitrateMode { get; set; }
        public int Quality { get; set; }
        public int Port1 { get; set; }
        public int Port2 { get; set; }
        public string Stream2Res { get; set; }
        public int Stream2Fps { get; set; }
        public int Brightness { get; set; }
        public int Contrast { get; set; }
        public int Saturation { get; set; }
        public int Sharpness { get; set; }
        public string MulticastIp { get; set; }
        public string StreamingMethod { get; set; }
        public bool IsOnline { get; set; }

        public CameraModel()
        {
            Name = "";
            Group = "";
            Resolution = "1920x1080 (1080p FHD)";
            Codec = "H.265";
            Fps = 25;
            BitrateMode = "Variable bit rate";
            Quality = 60;
            Port1 = 8860;
            Port2 = 8866;
            Stream2Res = "640x360";
            Stream2Fps = 25;
            Brightness = 50;
            Contrast = 50;
            Saturation = 50;
            Sharpness = 50;
            MulticastIp = "0.0.0.0";
            StreamingMethod = "RTP/RTSP/TCP";
            IsOnline = true;
        }

        public CameraModel Clone()
        {
            return (CameraModel)this.MemberwiseClone();
        }
    }
}

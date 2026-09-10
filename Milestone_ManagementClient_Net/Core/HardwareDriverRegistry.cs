using System;
using System.Collections.Generic;
using System.Linq;

namespace MilestoneManagementClient.Core
{
    public class DriverInfo
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string DriverName { get; set; }
        public string DriverVersion { get; set; }
        public string Protocol { get; set; }
        public int DefaultPort { get; set; }
        public string DefaultModel { get; set; }
        public string SupportedFeatures { get; set; }

        public DriverInfo(int id, string brand, string driverName, string version, string protocol, int defaultPort, string defaultModel, string features)
        {
            Id = id;
            Brand = brand;
            DriverName = driverName;
            DriverVersion = version;
            Protocol = protocol;
            DefaultPort = defaultPort;
            DefaultModel = defaultModel;
            SupportedFeatures = features;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", DriverName, DriverVersion);
        }
    }

    public static class HardwareDriverRegistry
    {
        private static readonly List<DriverInfo> _drivers = new List<DriverInfo>
        {
            new DriverInfo(1, "Universal ONVIF", "Universal ONVIF Conformance Driver", "Profile S/G/T/M v2025 R3", "ONVIF / SOAP", 80, "Generic Profile S/G/T Camera", "Live Video, Audio In/Out, PTZ, AI Metadata, Edge Storage"),
            new DriverInfo(2, "Axis Communications", "Axis Network Camera Driver Framework", "v12.4.0 ARTPEC-7/8", "VAPIX XML / ONVIF", 80, "AXIS P1455-LE 4K Network Camera", "VAPIX, ARTPEC Analytics, Radar, Audio Speaker, PTZ Guard Tour"),
            new DriverInfo(3, "Hikvision", "HikVision Generic IP Device Driver", "v2025.2 ISAPI / ColorVu / AcuSense", "Hikvision ISAPI / ONVIF", 8000, "DS-2CD2387G2-LU (4K ColorVu)", "ISAPI, ColorVu 24/7 Color, AcuSense Human/Vehicle AI, TandemVu PTZ"),
            new DriverInfo(4, "Dahua Technology", "Dahua Technology Universal ONVIF Driver", "v2025.1 WizMind / DH-IP", "Dahua RPC / ONVIF", 37777, "IPC-HFW5842E-ZE 4K WizMind AI Bullet", "WizMind AI, TiOC Active Deterrence, Full-Color, Thermal Alarm"),
            new DriverInfo(5, "Hanwha Vision", "Hanwha Vision / Wisenet Driver Framework", "v8.1.0 X/P/Q Series SUNAPI", "SUNAPI / ONVIF", 80, "Hanwha XND-9082RV 4K AI Dome", "SUNAPI, BestShot AI, Modular PTZ, Extreme WDR, Forensic Search"),
            new DriverInfo(6, "Bosch Security", "Bosch Dinion & Autodome Driver Framework", "v6.50.0 RCP+ / IP", "Bosch RCP+ over IP", 80, "Bosch AUTODOME IP 5000i PTZ", "RCP+ Protocol, AVIOTEC Video Fire Detection, Intelligent Video Analytics"),
            new DriverInfo(7, "Panasonic / i-PRO", "Panasonic / i-PRO Extreme Driver Framework", "v7.2.0 WV-Series", "Panasonic HD / ONVIF", 80, "i-PRO WV-S2572L 4K Extreme Dome", "AI Engine On-edge, Multi-Sensor, Auto-Tracking, Extreme Low Light"),
            new DriverInfo(8, "Sony", "Sony SNC Series IP Driver Framework", "v4.3.0 Gen-5/6", "Sony CGI / ONVIF", 80, "Sony SNC-VB640 Full HD Bullet", "Sony Exmor R, DEPA Advanced Analytics, View-DR Wide Dynamic Range"),
            new DriverInfo(9, "Pelco", "Pelco Sarix & Spectra IP Driver Framework", "v5.1.0 Pelco API", "Pelco API / ONVIF", 80, "Pelco Spectra Enhanced 4K PTZ", "SureVision 3.0, Optera 360 Dewarping, Smart Compression, Heavy-Duty"),
            new DriverInfo(10, "Uniview", "Uniview Universal IPC Driver Framework", "v3.4.0 UNV Protocol", "UNV Protocol / ONVIF", 80, "Uniview IPC2328SB 4K Prime Bullet", "LightHunter, Tri-Guard, ColorHunter, Smart Intrusion Prevention"),
            new DriverInfo(11, "FLIR Systems", "FLIR Thermal & Dual-Sensor Driver Framework", "v4.0.0 Nexus Thermal", "Nexus / Thermal CGI", 80, "FLIR Saros DH-390 Thermal Dome", "Radiometric Thermal, MSX Image Blending, Perimeter Detection"),
            new DriverInfo(12, "Mobotix", "Mobotix MxPEG / DualDome Driver Framework", "v5.2.0 MxPEG", "MxPEG / HTTP", 80, "Mobotix M73 Allround Dual Modular", "MxPEG Compression, Moonlight Sensor, Decentralized Edge Analytics"),
            new DriverInfo(13, "Vivotek", "Vivotek Network Camera Driver Framework", "v6.0.0 VAST/IP", "VAST Protocol / ONVIF", 80, "Vivotek MS9321-EHV Multi-Sensor Panoramic", "Smart Stream III, VAST Security Station, 180 Panoramic View"),
            new DriverInfo(14, "Avigilon", "Avigilon H4/H5/H6 HD Camera Driver Framework", "v5.4.0", "Avigilon Control Protocol", 80, "Avigilon 8.0C-H5A-BO1 4K Bullet", "Next-Generation Video Analytics (NGVA), Unusual Activity Detection"),
            new DriverInfo(15, "Canon", "Canon Network Camera Driver Framework", "v4.1.0 VB-Series", "Canon Protocol", 80, "Canon VB-H45 PTZ Network Camera", "DIGIC DV III, Ultra-Low Dispersion Lens, Auto Tracking"),
            new DriverInfo(16, "ACTi", "ACTi Unified IP Camera Driver Framework", "v5.0.0", "ACTi Unified SDK", 80, "ACTi E46A 3MP Vandal Dome", "Superior Low Light, Adaptive IR, Extreme WDR"),
            new DriverInfo(17, "Arecont Vision", "Arecont Vision SurroundVideo Multi-Sensor Driver", "v4.2.0", "Arecont AV Protocol", 80, "Arecont AV12186DN 180 Multi-Sensor", "SurroundVideo Panoramic, Dual-Sensor, True Day/Night"),
            new DriverInfo(18, "Honeywell", "Honeywell Performance & equIP Series Driver", "v3.8.0", "MAXPRO / ONVIF", 80, "Honeywell HC30WB4R2 4MP IR Bullet", "MAXPRO Integration, True WDR, Smart IR LEDs"),
            new DriverInfo(19, "Geovision", "Geovision GV-IP Speed Dome & Target Driver", "v4.0.0", "GV-IP Protocol", 80, "Geovision GV-SD2322-IR 33x PTZ", "GV-VMS Sync, Super Low Lux, 33x Optical Zoom"),
            new DriverInfo(20, "Vicon", "Vicon Roughneck & Surveyor Driver", "v3.5.0", "ViconNet Protocol", 80, "Vicon V944D-W312HIR 4MP Dome", "Valerus Integration, Quad Stream, Smart Encoding"),
            new DriverInfo(24, "Milestone Interconnect", "Milestone Direct XProtect Recorder Node", "v2025 R3 Interconnect", "VideoOS Interconnect", 7563, "XProtect Corporate / Expert Recording Node", "Inter-recorder Cascading, Central Alarm Sync, Remote Playback"),
            new DriverInfo(25, "Alcon", "Alcon Smart Surveillance Driver Framework", "v5.2.0 Smart-IPC / ONVIF Profile S/G", "Alcon Protocol / ONVIF", 80, "AL-5001-MPC-HD4DK-1 4K AI Bullet", "Smart Analytics, H.265+, True WDR, Starlight Low Light, Face Detection"),
            new DriverInfo(26, "CP PLUS", "CP PLUS Coral & Orange IP Driver Framework", "v4.5.0 CP-Plus ONVIF", "CP PLUS Protocol / ONVIF", 80, "CP-UNC-TA41ZL4-VMD 4K Bullet", "InstaStream, Ultra Clarity, AI Human Detection, Active Deterrence"),
            new DriverInfo(27, "TP-Link / Tapo", "TP-Link Tapo & VIGI Surveillance Driver", "v3.1.0 VIGI Protocol", "Tapo RTSP / ONVIF", 554, "Tapo C320WS / VIGI C340 4K", "Smart Detection, Two-Way Audio, Night Vision, High Frame Rate")
        };

        public static List<DriverInfo> GetAllDrivers()
        {
            return _drivers;
        }

        public static string[] GetAllDriverNames()
        {
            return _drivers.Select(d => d.ToString()).ToArray();
        }

        public static DriverInfo GetDriverByBrand(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand)) return _drivers[0];
            return _drivers.FirstOrDefault(d => d.Brand.IndexOf(brand, StringComparison.OrdinalIgnoreCase) >= 0) ?? _drivers[0];
        }

        public static DriverInfo GetDriverByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return _drivers[0];
            return _drivers.FirstOrDefault(d => d.DriverName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0 || d.ToString().IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) ?? _drivers[0];
        }
    }
}

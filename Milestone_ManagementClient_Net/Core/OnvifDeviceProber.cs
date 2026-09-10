using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MilestoneManagementClient.Core
{
    public class OnvifDeviceInfo
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string FirmwareVersion { get; set; }
        public string SerialNumber { get; set; }
        public string HardwareId { get; set; }
        public string SnapshotUri { get; set; }
        public string StreamUri { get; set; }
        public int HttpPort { get; set; }
        public int RtspPort { get; set; }
        public string DriverName { get; set; }
    }

    public static class OnvifDeviceProber
    {
        public static string ResolveBrandFromMac(string mac)
        {
            if (string.IsNullOrEmpty(mac)) return "Universal ONVIF";
            string clean = mac.Replace(":", "").Replace("-", "").ToUpper();
            if (clean.Length < 6) return "Universal ONVIF";
            string oui = clean.Substring(0, 6);

            // Hikvision
            if (oui == "001A8C" || oui == "4419B6" || oui == "BC5436" || oui == "4CBD8F" || oui == "C42F90" || oui == "D46E5C" || oui == "E0508B" || oui == "F84DFC" || oui == "54C415")
                return "Hikvision";

            // Axis
            if (oui == "00408C" || oui == "ACCC8E" || oui == "B8A44F")
                return "Axis Communications";

            // Dahua
            if (oui == "38AF29" || oui == "4C11BF" || oui == "9002A9" || oui == "B0C554" || oui == "A0BDCD")
                return "Dahua Technology";

            // Hanwha
            if (oui == "000918" || oui == "00166C" || oui == "F4D9FB")
                return "Hanwha Vision";

            // CP PLUS
            if (oui == "A41437" || oui == "3C83B5" || oui == "687251")
                return "CP PLUS";

            // Uniview
            if (oui == "0014D1" || oui == "B4A382")
                return "Uniview";

            // Bosch
            if (oui == "001EC0" || oui == "000463")
                return "Bosch Security";

            // TP-Link / Tapo
            if (oui == "14A78B" || oui == "50D4F7" || oui == "B09575")
                return "TP-Link / Tapo";

            // Vivotek
            if (oui == "00075F" || oui == "0002D1")
                return "Vivotek";

            // Panasonic
            if (oui == "00115C" || oui == "0080F0")
                return "Panasonic / i-PRO";

            // Alcon
            if (oui == "001A92" || oui == "70B3D5" || oui == "000B82" || oui == "2857BE")
                return "Alcon";

            return "Universal ONVIF";
        }

        public static OnvifDeviceInfo ProbeCamera(string ip, int port, string user, string pass)
        {
            var info = new OnvifDeviceInfo
            {
                Manufacturer = "Universal ONVIF",
                Model = "IP Network Camera",
                FirmwareVersion = "v2025 R3",
                SerialNumber = "SN-" + ip.Replace(".", ""),
                HttpPort = port > 0 ? port : 80,
                RtspPort = 554,
                DriverName = "Universal ONVIF Conformance Driver"
            };

            int[] portsToTry = (port > 0 && port != 80) ? new[] { port, 80, 8080, 8899, 8000, 37777 } : new[] { 80, 8080, 8899, 8000, 37777 };

            foreach (var p in portsToTry)
            {
                try
                {
                    string soapReq =
                        "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:tds=\"http://www.onvif.org/ver10/device/wsdl\">" +
                        "<soap:Body><tds:GetDeviceInformation /></soap:Body></soap:Envelope>";

                    string url = string.Format("http://{0}:{1}/onvif/device_service", ip, p);
                    string resp = SendSoapRequest(url, soapReq, user, pass, 350);

                    if (!string.IsNullOrEmpty(resp) && resp.Contains("GetDeviceInformationResponse"))
                    {
                        info.HttpPort = p;
                        info.Manufacturer = ExtractXmlTag(resp, "Manufacturer") ?? info.Manufacturer;
                        info.Model = ExtractXmlTag(resp, "Model") ?? info.Model;
                        info.FirmwareVersion = ExtractXmlTag(resp, "FirmwareVersion") ?? info.FirmwareVersion;
                        info.SerialNumber = ExtractXmlTag(resp, "SerialNumber") ?? info.SerialNumber;
                        info.HardwareId = ExtractXmlTag(resp, "HardwareId") ?? info.HardwareId;
                        info.DriverName = HardwareDriverRegistry.GetDriverByBrand(info.Manufacturer).DriverName;
                        break;
                    }
                }
                catch { }
            }

            return info;
        }

        public static string SendSoapRequest(string url, string soapBody, string user, string pass, int timeoutMs)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/soap+xml; charset=utf-8";
                req.Timeout = timeoutMs;
                req.ReadWriteTimeout = timeoutMs;

                if (!string.IsNullOrEmpty(user))
                {
                    req.Credentials = new NetworkCredential(user, pass);
                }

                byte[] data = Encoding.UTF8.GetBytes(soapBody);
                req.ContentLength = data.Length;
                using (var reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }

                using (var resp = (HttpWebResponse)req.GetResponse())
                using (var reader = new StreamReader(resp.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch { }
            return null;
        }

        public static string ExtractXmlTag(string xml, string tagName)
        {
            try
            {
                var match = Regex.Match(xml, @"<[^:]*:" + tagName + @"[^>]*>([^<]+)</[^:]*:" + tagName + @">", RegexOptions.IgnoreCase);
                if (match.Success) return match.Groups[1].Value.Trim();

                match = Regex.Match(xml, @"<" + tagName + @"[^>]*>([^<]+)</" + tagName + @">", RegexOptions.IgnoreCase);
                if (match.Success) return match.Groups[1].Value.Trim();
            }
            catch { }
            return null;
        }
    }
}

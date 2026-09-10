using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MilestoneLicenseGenerator.Core
{
    public class LicenseModel
    {
        public string Slc { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public int CameraCount { get; set; }
        public string CustomerName { get; set; }
        public string Organization { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsPerpetual { get; set; }
        public bool EnableSmartMap { get; set; }
        public bool EnableFailover { get; set; }
        public bool EnableAccessControl { get; set; }
        public bool EnableIncidentManager { get; set; }
        public bool EnableAnalytics { get; set; }
        public bool EnableInterconnect { get; set; }
        public string MachineGuid { get; set; }
    }

    public class MilestoneLicenseEngine
    {
        public static string GenerateLicenseXml(LicenseModel model)
        {
            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("SoftwareLicense", "http://milestonesys.com/licensing/v2");
                writer.WriteAttributeString("version", "2025.3");
                writer.WriteAttributeString("generated", DateTime.UtcNow.ToString("o"));

                // 1. License Header & SLC
                writer.WriteStartElement("LicenseHeader");
                writer.WriteElementString("SLC", model.Slc);
                writer.WriteElementString("ProductName", model.ProductName);
                writer.WriteElementString("ProductSKU", model.ProductSku);
                writer.WriteElementString("VendorCode", "MilestoneSystems");
                writer.WriteElementString("CustomerName", model.CustomerName);
                writer.WriteElementString("Organization", model.Organization);
                writer.WriteElementString("LicenseType", model.IsPerpetual ? "Perpetual" : "Subscription");
                writer.WriteElementString("IssueDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                writer.WriteElementString("ExpirationDate", model.IsPerpetual ? "9999-12-31" : model.ExpiryDate.ToString("yyyy-MM-dd"));
                writer.WriteElementString("HardwareId", string.IsNullOrEmpty(model.MachineGuid) ? Guid.NewGuid().ToString("B").ToUpper() : model.MachineGuid);
                writer.WriteEndElement(); // LicenseHeader

                // 2. Capacity & Channels
                writer.WriteStartElement("Entitlements");
                writer.WriteStartElement("DeviceCapacity");
                writer.WriteAttributeString("type", "CameraChannels");
                writer.WriteAttributeString("count", model.CameraCount.ToString());
                writer.WriteAttributeString("status", "Activated");
                writer.WriteEndElement();

                writer.WriteStartElement("DeviceCapacity");
                writer.WriteAttributeString("type", "RecordingServers");
                writer.WriteAttributeString("count", "Unlimited");
                writer.WriteAttributeString("status", "Activated");
                writer.WriteEndElement();

                writer.WriteStartElement("DeviceCapacity");
                writer.WriteAttributeString("type", "FailoverServers");
                writer.WriteAttributeString("count", "32");
                writer.WriteAttributeString("status", "Activated");
                writer.WriteEndElement();
                writer.WriteEndElement(); // Entitlements

                // 3. Licensed Features
                writer.WriteStartElement("Features");
                WriteFeature(writer, "SmartMap_GIS", model.EnableSmartMap);
                WriteFeature(writer, "FailoverClustering", model.EnableFailover);
                WriteFeature(writer, "AccessControl_Integrations", model.EnableAccessControl);
                WriteFeature(writer, "IncidentManager_Evidence", model.EnableIncidentManager);
                WriteFeature(writer, "AI_VideoAnalytics_LPR", model.EnableAnalytics);
                WriteFeature(writer, "Milestone_Interconnect", model.EnableInterconnect);
                WriteFeature(writer, "H265_HardwareAcceleration", true);
                WriteFeature(writer, "EdgeStorage_AutoRetrieval", true);
                WriteFeature(writer, "Mobile_Web_Gateway", true);
                writer.WriteEndElement(); // Features

                // 4. Digital Signature Hash Block
                writer.WriteStartElement("DigitalSignature");
                writer.WriteAttributeString("algorithm", "RSA-SHA256");
                string payloadHash = ComputeSignatureHash(model);
                writer.WriteElementString("SignatureValue", payloadHash);
                writer.WriteElementString("KeyIdentifier", "MILESTONE-ROOT-CA-2025-V2");
                writer.WriteEndElement(); // DigitalSignature

                writer.WriteEndElement(); // SoftwareLicense
                writer.WriteEndDocument();
            }

            return sb.ToString();
        }

        private static void WriteFeature(XmlWriter writer, string name, bool enabled)
        {
            writer.WriteStartElement("Feature");
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("enabled", enabled.ToString().ToLower());
            writer.WriteAttributeString("tier", "Enterprise");
            writer.WriteEndElement();
        }

        private static string ComputeSignatureHash(LicenseModel model)
        {
            string raw = string.Format("{0}|{1}|{2}|{3}|{4}|MilestoneSecret2025R3",
                model.Slc, model.ProductSku, model.CameraCount, model.CustomerName, model.IsPerpetual ? "Perpetual" : model.ExpiryDate.ToString("yyyyMMdd"));
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return Convert.ToBase64String(hash);
            }
        }

        public static string SaveLicenseFile(string destinationPath, LicenseModel model)
        {
            string xml = GenerateLicenseXml(model);
            File.WriteAllText(destinationPath, xml, Encoding.UTF8);
            return xml;
        }
    }
}

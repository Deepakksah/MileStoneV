using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Server.Licensing;

public sealed class RecorderLicense
{
	public static class StatusConstants
	{
		public static readonly Guid Unlicensed = new Guid("D2A87979-EC8E-4C42-93E6-D12B49774B10");

		public static readonly Guid Licensed = new Guid("A4C746E3-7F89-45A9-9C1E-71F41D544359");
	}

	private class DeviceLicenseInfo
	{
		public string LicenseType { get; private set; }

		public long ExpiryTicks { get; private set; }

		public DeviceLicenseInfo(string licenseType, long expiryTicks)
		{
			LicenseType = licenseType;
			ExpiryTicks = expiryTicks;
		}
	}

	private Hashtable _hardwareLicenses = new Hashtable();

	private Dictionary<Guid, Dictionary<Guid, DeviceLicenseInfo>> _deviceLicenses = new Dictionary<Guid, Dictionary<Guid, DeviceLicenseInfo>>();

	public Guid RecorderId { get; set; }

	public Guid Status { get; set; }

	public DateTime ExpiryDate { get; set; }

	public List<string> FeaturesSupported { get; private set; }

	public RecorderLicense(Guid recorderId)
	{
		RecorderId = recorderId;
		Status = StatusConstants.Unlicensed;
		ExpiryDate = DateTime.MinValue;
		FeaturesSupported = new List<string>();
	}

	public void AddHardwareLicense(Guid hardwareId, LicenseBase license)
	{
		_hardwareLicenses[hardwareId] = license;
	}

	public void AddDeviceLicense(Guid hardwareId, Guid deviceId, string licenseType, long expiryTicks)
	{
		if (!_deviceLicenses.ContainsKey(hardwareId))
		{
			_deviceLicenses.Add(hardwareId, new Dictionary<Guid, DeviceLicenseInfo>());
		}
		_deviceLicenses[hardwareId][deviceId] = new DeviceLicenseInfo(licenseType, expiryTicks);
	}

	public byte[] GetResponse()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("configuration");
			xmlTextWriter.WriteElementString("issued", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
			if (ExpiryDate > DateTime.MinValue)
			{
				xmlTextWriter.WriteElementString("expires", ExpiryDate.ToString("u", CultureInfo.InvariantCulture));
			}
			xmlTextWriter.WriteStartElement("recorder");
			xmlTextWriter.WriteAttributeString("id", RecorderId.ToString());
			xmlTextWriter.WriteAttributeString("status", Status.ToString());
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("licenses");
			foreach (object key in _hardwareLicenses.Keys)
			{
				xmlTextWriter.WriteStartElement("license");
				if (_hardwareLicenses[key] is LicenseBase licenseBase)
				{
					xmlTextWriter.WriteElementString("type", licenseBase.Type);
					xmlTextWriter.WriteElementString("mac", licenseBase.MacAddress);
					licenseBase.WriteLicenseDetails(xmlTextWriter);
					xmlTextWriter.WriteStartElement("devices");
					if (_deviceLicenses.TryGetValue((Guid)key, out var value))
					{
						foreach (Guid key2 in value.Keys)
						{
							DeviceLicenseInfo deviceLicenseInfo = value[key2];
							if (deviceLicenseInfo != null)
							{
								xmlTextWriter.WriteStartElement("device");
								xmlTextWriter.WriteElementString("id", key2.ToString());
								xmlTextWriter.WriteElementString("type", deviceLicenseInfo.LicenseType);
								xmlTextWriter.WriteElementString("expire", deviceLicenseInfo.ExpiryTicks.ToString(CultureInfo.InvariantCulture));
								xmlTextWriter.WriteEndElement();
							}
						}
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("features");
			foreach (string item in FeaturesSupported)
			{
				xmlTextWriter.WriteElementString("feature", item);
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}
}

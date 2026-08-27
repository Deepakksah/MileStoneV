using System;
using System.Globalization;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

[XmlRoot("Result")]
public class FirmwareUpgradeTaskResult
{
	[XmlElement("HardwareId", DataType = "string")]
	public string InternalHardwareId { get; set; }

	[XmlIgnore]
	public Guid HardwareId
	{
		get
		{
			return new Guid(InternalHardwareId);
		}
		set
		{
			InternalHardwareId = value.ToString();
		}
	}

	[XmlElement("HardwareName", DataType = "string")]
	public string InternalHardwareName { get; set; }

	[XmlIgnore]
	public string HardwareName
	{
		get
		{
			return InternalHardwareName;
		}
		set
		{
			InternalHardwareName = value;
		}
	}

	[XmlElement("RecordingServerId", DataType = "string")]
	public string InternalRecordingServerId { get; set; }

	[XmlIgnore]
	public Guid RecordingServerId
	{
		get
		{
			return new Guid(InternalRecordingServerId);
		}
		set
		{
			InternalRecordingServerId = value.ToString();
		}
	}

	[XmlElement("InternalWarningCode", DataType = "string")]
	public string InternalWarningCode { get; set; }

	[XmlIgnore]
	public int WarningCode
	{
		get
		{
			int result = 0;
			if (!int.TryParse(InternalWarningCode, out result))
			{
				return 0;
			}
			return result;
		}
		set
		{
			InternalWarningCode = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("WarningMessage", DataType = "string")]
	public string InternalWarningMessage { get; set; }

	[XmlIgnore]
	public string WarningMessage
	{
		get
		{
			return InternalWarningMessage;
		}
		set
		{
			InternalWarningMessage = value;
		}
	}

	[XmlElement("NewFirmwareVersion", DataType = "string")]
	public string InternalNewFirmwareVersion { get; set; }

	[XmlIgnore]
	public string NewFirmwareVersion
	{
		get
		{
			return InternalNewFirmwareVersion;
		}
		set
		{
			InternalNewFirmwareVersion = value;
		}
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static FirmwareUpgradeTaskResult FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<FirmwareUpgradeTaskResult>(xml);
	}
}

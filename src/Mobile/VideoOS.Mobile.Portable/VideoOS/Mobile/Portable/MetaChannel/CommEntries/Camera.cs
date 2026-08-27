using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Camera
{
	public const string CameraIdXmlName = "CameraId";

	public const string CameraNameXmlName = "CameraName";

	public const string RsIdXmlName = "RSId";

	public const string RsNameXmlName = "RSName";

	[JsonProperty("CameraId")]
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid CameraId { get; set; }

	[JsonProperty("CameraName")]
	[XmlAttribute(AttributeName = "CameraName")]
	public string CameraName { get; set; }

	[JsonProperty("RSId")]
	[XmlAttribute(AttributeName = "RSId")]
	public Guid RecordingServerId { get; set; }

	[JsonProperty("RSName")]
	[XmlAttribute(AttributeName = "RSName")]
	public string RecordingServerName { get; set; }
}

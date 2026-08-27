using System;
using System.Xml.Serialization;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CameraRelatedIds
{
	[XmlAttribute(AttributeName = "CameraId")]
	public Guid CameraId { get; set; }

	[XmlAttribute(AttributeName = "RelatedMicrophone")]
	public Guid RelatedMicrohponeId { get; set; }

	[XmlAttribute(AttributeName = "RelatedSpeaker")]
	public Guid RelatedSpeakerId { get; set; }
}

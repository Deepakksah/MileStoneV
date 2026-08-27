using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("RecorderInfo: {Name} ({RecorderId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class RecorderInfo
{
	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public Guid ServiceId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string HostName { get; set; }

	[DataMember]
	public string WebServerUri { get; set; }

	[DataMember]
	public string TimeZoneName { get; set; }

	[DataMember]
	public int DefaultRecordingTimeSeconds { get; set; }

	[DataMember]
	public string XmlEncoding { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	[DataMember]
	public CameraInfo[] Cameras { get; set; }

	[DataMember]
	public InputInfo[] Inputs { get; set; }

	[DataMember]
	public OutputInfo[] Outputs { get; set; }

	[DataMember]
	public MicrophoneInfo[] Microphones { get; set; }

	[DataMember]
	public SpeakerInfo[] Speakers { get; set; }

	[DataMember]
	public MetadataDeviceInfo[] MetadataDevices { get; set; }

	[DataMember]
	public HardwareInfo[] Hardware { get; set; }
}

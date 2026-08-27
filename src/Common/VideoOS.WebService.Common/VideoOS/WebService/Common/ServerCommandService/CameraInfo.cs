using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class CameraInfo : DeviceInfo
{
	[DataMember]
	public double MaxFPS { get; set; }

	[DataMember]
	public bool BrowsableStream { get; set; }

	[DataMember]
	public StreamInfo[] Streams { get; set; }

	[DataMember]
	public TrackInfo[] Tracks { get; set; }

	[DataMember]
	public CameraSecurityInfo CameraSecurity { get; set; }

	[DataMember]
	public PtzInfo PtzSettings { get; set; }

	[DataMember]
	public IpixInfo IpixSettings { get; set; }

	[DataMember]
	public PanoramicLensInfo PanoramicLensSettings { get; set; }

	[DataMember]
	public bool MulticastEnabled { get; set; }

	[DataMember]
	public bool EdgeStorageSupported { get; set; }

	[DataMember]
	public bool EdgeStoragePlayback { get; set; }

	[DataMember]
	public int StopManualRecordingSeconds { get; set; }
}

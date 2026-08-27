using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class SpeakerInfo : DeviceInfo
{
	[DataMember]
	public TrackInfo[] Tracks { get; set; }

	[DataMember]
	public SpeakerSecurityInfo SpeakerSecurity { get; set; }

	[DataMember]
	public bool MulticastEnabled { get; set; }

	[DataMember]
	public bool EdgeStorageSupported { get; set; }

	[DataMember]
	public bool EdgeStoragePlayback { get; set; }

	[DataMember]
	public int StopManualRecordingSeconds { get; set; }
}

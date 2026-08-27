using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SpeakerInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SpeakerInfo : DeviceInfo
{
	private bool EdgeStoragePlaybackField;

	private bool EdgeStorageSupportedField;

	private bool MulticastEnabledField;

	private SpeakerSecurityInfo SpeakerSecurityField;

	private int StopManualRecordingSecondsField;

	private TrackInfo[] TracksField;

	[DataMember]
	public bool EdgeStoragePlayback
	{
		get
		{
			return EdgeStoragePlaybackField;
		}
		set
		{
			EdgeStoragePlaybackField = value;
		}
	}

	[DataMember]
	public bool EdgeStorageSupported
	{
		get
		{
			return EdgeStorageSupportedField;
		}
		set
		{
			EdgeStorageSupportedField = value;
		}
	}

	[DataMember]
	public bool MulticastEnabled
	{
		get
		{
			return MulticastEnabledField;
		}
		set
		{
			MulticastEnabledField = value;
		}
	}

	[DataMember]
	public SpeakerSecurityInfo SpeakerSecurity
	{
		get
		{
			return SpeakerSecurityField;
		}
		set
		{
			SpeakerSecurityField = value;
		}
	}

	[DataMember]
	public int StopManualRecordingSeconds
	{
		get
		{
			return StopManualRecordingSecondsField;
		}
		set
		{
			StopManualRecordingSecondsField = value;
		}
	}

	[DataMember]
	public TrackInfo[] Tracks
	{
		get
		{
			return TracksField;
		}
		set
		{
			TracksField = value;
		}
	}
}

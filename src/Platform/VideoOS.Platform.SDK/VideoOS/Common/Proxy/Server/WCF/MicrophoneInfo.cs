using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MicrophoneInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MicrophoneInfo : DeviceInfo, IMicrophoneInfo, IMediaDeviceInfo, IDeviceInfo
{
	private bool EdgeStoragePlaybackField;

	private bool EdgeStorageSupportedField;

	private MicrophoneSecurityInfo MicrophoneSecurityField;

	private bool MulticastEnabledField;

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
	public MicrophoneSecurityInfo MicrophoneSecurity
	{
		get
		{
			return MicrophoneSecurityField;
		}
		set
		{
			MicrophoneSecurityField = value;
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

	ITrackInfo[] IMediaDeviceInfo.Tracks
	{
		get
		{
			return Tracks;
		}
		set
		{
			Tracks = (TrackInfo[])value;
		}
	}

	IMicrophoneSecurityInfo IMicrophoneInfo.MicrophoneSecurity
	{
		get
		{
			return MicrophoneSecurity;
		}
		set
		{
			MicrophoneSecurity = (MicrophoneSecurityInfo)value;
		}
	}
}

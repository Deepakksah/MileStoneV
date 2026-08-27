using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CameraInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CameraInfo : DeviceInfo
{
	private bool BrowsableStreamField;

	private CameraSecurityInfo CameraSecurityField;

	private bool EdgeStoragePlaybackField;

	private bool EdgeStorageSupportedField;

	private IpixInfo IpixSettingsField;

	private double MaxFPSField;

	private bool MulticastEnabledField;

	private PanoramicLensInfo PanoramicLensSettingsField;

	private PtzInfo PtzSettingsField;

	private int StopManualRecordingSecondsField;

	private StreamInfo[] StreamsField;

	private TrackInfo[] TracksField;

	[DataMember]
	public bool BrowsableStream
	{
		get
		{
			return BrowsableStreamField;
		}
		set
		{
			BrowsableStreamField = value;
		}
	}

	[DataMember]
	public CameraSecurityInfo CameraSecurity
	{
		get
		{
			return CameraSecurityField;
		}
		set
		{
			CameraSecurityField = value;
		}
	}

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
	public IpixInfo IpixSettings
	{
		get
		{
			return IpixSettingsField;
		}
		set
		{
			IpixSettingsField = value;
		}
	}

	[DataMember]
	public double MaxFPS
	{
		get
		{
			return MaxFPSField;
		}
		set
		{
			MaxFPSField = value;
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
	public PanoramicLensInfo PanoramicLensSettings
	{
		get
		{
			return PanoramicLensSettingsField;
		}
		set
		{
			PanoramicLensSettingsField = value;
		}
	}

	[DataMember]
	public PtzInfo PtzSettings
	{
		get
		{
			return PtzSettingsField;
		}
		set
		{
			PtzSettingsField = value;
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
	public StreamInfo[] Streams
	{
		get
		{
			return StreamsField;
		}
		set
		{
			StreamsField = value;
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

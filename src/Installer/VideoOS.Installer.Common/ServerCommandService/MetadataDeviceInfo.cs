using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MetadataDeviceInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MetadataDeviceInfo : DeviceInfo
{
	private bool EdgeStoragePlaybackField;

	private bool EdgeStorageSupportedField;

	private MetadataDeviceSecurityInfo MetadataDeviceSecurityField;

	private MetadataTypeInfo[] MetadataTypesField;

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
	public MetadataDeviceSecurityInfo MetadataDeviceSecurity
	{
		get
		{
			return MetadataDeviceSecurityField;
		}
		set
		{
			MetadataDeviceSecurityField = value;
		}
	}

	[DataMember]
	public MetadataTypeInfo[] MetadataTypes
	{
		get
		{
			return MetadataTypesField;
		}
		set
		{
			MetadataTypesField = value;
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
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "MetadataDeviceInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class MetadataDeviceInfo : DeviceInfo, IMetadataDeviceInfo, IMediaDeviceInfo, IDeviceInfo
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

	IMetadataDeviceSecurityInfo IMetadataDeviceInfo.MetadataDeviceSecurity
	{
		get
		{
			return MetadataDeviceSecurity;
		}
		set
		{
			MetadataDeviceSecurity = (MetadataDeviceSecurityInfo)value;
		}
	}

	IMetadataTypeInfo[] IMetadataDeviceInfo.MetadataTypes
	{
		get
		{
			return MetadataTypes;
		}
		set
		{
			MetadataTypes = (MetadataTypeInfo[])value;
		}
	}
}

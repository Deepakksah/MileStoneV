using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class MetadataDeviceInfo : DeviceInfo, IMetadataDeviceInfo, IMediaDeviceInfo, IDeviceInfo
{
	private TrackInfo[] tracksField;

	private MetadataDeviceSecurityInfo metadataDeviceSecurityField;

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

	public bool EdgeStorageSupported { get; set; }

	public bool EdgeStoragePlayback { get; set; }

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

	public MetadataTypeInfo[] MetadataTypes { get; set; }

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

	public bool MulticastEnabled { get; set; }

	public int StopManualRecordingSeconds { get; set; }

	public TrackInfo[] Tracks
	{
		get
		{
			return tracksField;
		}
		set
		{
			tracksField = value;
		}
	}

	public MetadataDeviceSecurityInfo MetadataDeviceSecurity
	{
		get
		{
			return metadataDeviceSecurityField;
		}
		set
		{
			metadataDeviceSecurityField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DeviceStatisticsBase", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
[KnownType(typeof(AudioDeviceStatistics))]
[KnownType(typeof(MetadataDeviceStatistics))]
[KnownType(typeof(VideoDeviceStatistics))]
public class DeviceStatisticsBase : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid DeviceIdField;

	private ulong UsedSpaceInBytesField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public ulong UsedSpaceInBytes
	{
		get
		{
			return UsedSpaceInBytesField;
		}
		set
		{
			UsedSpaceInBytesField = value;
		}
	}
}

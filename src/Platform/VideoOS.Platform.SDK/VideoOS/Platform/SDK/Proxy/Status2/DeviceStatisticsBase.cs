using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[XmlInclude(typeof(MetadataDeviceStatistics))]
[XmlInclude(typeof(AudioDeviceStatistics))]
[XmlInclude(typeof(VideoDeviceStatistics))]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class DeviceStatisticsBase
{
	private Guid deviceIdField;

	private ulong usedSpaceInBytesField;

	public Guid DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	public ulong UsedSpaceInBytes
	{
		get
		{
			return usedSpaceInBytesField;
		}
		set
		{
			usedSpaceInBytesField = value;
		}
	}
}

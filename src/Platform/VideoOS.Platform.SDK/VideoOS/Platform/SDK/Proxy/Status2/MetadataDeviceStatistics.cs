using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class MetadataDeviceStatistics : DeviceStatisticsBase
{
	private MetadataStreamStatistics[] metadataStreamStatisticsArrayField;

	public MetadataStreamStatistics[] MetadataStreamStatisticsArray
	{
		get
		{
			return metadataStreamStatisticsArrayField;
		}
		set
		{
			metadataStreamStatisticsArrayField = value;
		}
	}
}

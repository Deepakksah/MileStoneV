using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class JPEGData
{
	private DateTime timeField;

	private byte[] dataField;

	[XmlElement(Order = 0)]
	public DateTime Time
	{
		get
		{
			return timeField;
		}
		set
		{
			timeField = value;
		}
	}

	[XmlElement(DataType = "base64Binary", Order = 1)]
	public byte[] Data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SmartSearchStatus
{
	private DateTime currentSearchTimeField;

	private SmartSearchStatusType statusField;

	[XmlElement(Order = 0)]
	public DateTime CurrentSearchTime
	{
		get
		{
			return currentSearchTimeField;
		}
		set
		{
			currentSearchTimeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public SmartSearchStatusType Status
	{
		get
		{
			return statusField;
		}
		set
		{
			statusField = value;
		}
	}
}

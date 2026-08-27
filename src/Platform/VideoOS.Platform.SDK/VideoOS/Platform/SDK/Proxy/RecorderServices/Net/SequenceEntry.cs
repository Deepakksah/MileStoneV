using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SequenceEntry
{
	private DateTime timeBeginField;

	private DateTime timeTriggedField;

	private DateTime timeEndField;

	[XmlElement(Order = 0)]
	public DateTime TimeBegin
	{
		get
		{
			return timeBeginField;
		}
		set
		{
			timeBeginField = value;
		}
	}

	[XmlElement(Order = 1)]
	public DateTime TimeTrigged
	{
		get
		{
			return timeTriggedField;
		}
		set
		{
			timeTriggedField = value;
		}
	}

	[XmlElement(Order = 2)]
	public DateTime TimeEnd
	{
		get
		{
			return timeEndField;
		}
		set
		{
			timeEndField = value;
		}
	}
}

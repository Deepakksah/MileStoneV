using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SequenceEntry
{
	private DateTime timeBeginField;

	private DateTime timeTriggedField;

	private DateTime timeEndField;

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

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SmartSearchStatus
{
	private DateTime currentSearchTimeField;

	private SmartSearchStatusType statusField;

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

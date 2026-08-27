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
public class TimeDuration
{
	private long microSecondsField;

	public long MicroSeconds
	{
		get
		{
			return microSecondsField;
		}
		set
		{
			microSecondsField = value;
		}
	}
}

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
public class MotionAreaInfos
{
	private Size resolutionField;

	private MotionAreaInfo[] areasField;

	public Size Resolution
	{
		get
		{
			return resolutionField;
		}
		set
		{
			resolutionField = value;
		}
	}

	public MotionAreaInfo[] Areas
	{
		get
		{
			return areasField;
		}
		set
		{
			areasField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Server.Proxy.Recorder;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SmartSearchResult
{
	private DateTime imageTimeField;

	private ImageMask motionMaskField;

	public DateTime ImageTime
	{
		get
		{
			return imageTimeField;
		}
		set
		{
			imageTimeField = value;
		}
	}

	public ImageMask MotionMask
	{
		get
		{
			return motionMaskField;
		}
		set
		{
			motionMaskField = value;
		}
	}
}

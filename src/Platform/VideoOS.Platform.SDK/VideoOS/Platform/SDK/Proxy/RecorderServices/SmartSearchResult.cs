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
public class SmartSearchResult
{
	private DateTime imageTimeField;

	private ImageMask motionMaskField;

	private MotionAreaInfos motionAreasField;

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

	public MotionAreaInfos MotionAreas
	{
		get
		{
			return motionAreasField;
		}
		set
		{
			motionAreasField = value;
		}
	}
}

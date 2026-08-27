using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class SmartSearchResult
{
	private DateTime imageTimeField;

	private ImageMask motionMaskField;

	private MotionAreaInfos motionAreasField;

	[XmlElement(Order = 0)]
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

	[XmlElement(Order = 1)]
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

	[XmlElement(Order = 2)]
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

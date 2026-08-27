using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class MotionAreaInfos
{
	private Size resolutionField;

	private MotionAreaInfo[] areasField;

	[XmlElement(Order = 0)]
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

	[XmlArray(Order = 1)]
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

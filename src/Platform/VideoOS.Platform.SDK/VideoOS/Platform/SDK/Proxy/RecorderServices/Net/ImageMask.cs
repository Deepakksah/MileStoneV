using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class ImageMask
{
	private Size sizeField;

	private string maskField;

	[XmlElement(Order = 0)]
	public Size Size
	{
		get
		{
			return sizeField;
		}
		set
		{
			sizeField = value;
		}
	}

	[XmlElement(Order = 1)]
	public string Mask
	{
		get
		{
			return maskField;
		}
		set
		{
			maskField = value;
		}
	}
}

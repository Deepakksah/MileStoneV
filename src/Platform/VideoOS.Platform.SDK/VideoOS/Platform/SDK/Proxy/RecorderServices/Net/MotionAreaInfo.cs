using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class MotionAreaInfo
{
	private int xField;

	private int yField;

	private int widthField;

	private int heightField;

	[XmlElement(Order = 0)]
	public int X
	{
		get
		{
			return xField;
		}
		set
		{
			xField = value;
		}
	}

	[XmlElement(Order = 1)]
	public int Y
	{
		get
		{
			return yField;
		}
		set
		{
			yField = value;
		}
	}

	[XmlElement(Order = 2)]
	public int Width
	{
		get
		{
			return widthField;
		}
		set
		{
			widthField = value;
		}
	}

	[XmlElement(Order = 3)]
	public int Height
	{
		get
		{
			return heightField;
		}
		set
		{
			heightField = value;
		}
	}
}

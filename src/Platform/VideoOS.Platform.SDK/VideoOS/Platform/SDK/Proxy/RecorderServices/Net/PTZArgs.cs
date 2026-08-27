using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class PTZArgs
{
	private PTZPairDouble[] movementField;

	private PTZPairDouble[] speedField;

	private PTZPairBoolean[] automaticField;

	private bool normalizedField;

	[XmlElement("movement", Order = 0)]
	public PTZPairDouble[] movement
	{
		get
		{
			return movementField;
		}
		set
		{
			movementField = value;
		}
	}

	[XmlElement("speed", Order = 1)]
	public PTZPairDouble[] speed
	{
		get
		{
			return speedField;
		}
		set
		{
			speedField = value;
		}
	}

	[XmlElement("automatic", Order = 2)]
	public PTZPairBoolean[] automatic
	{
		get
		{
			return automaticField;
		}
		set
		{
			automaticField = value;
		}
	}

	[XmlElement(Order = 3)]
	public bool Normalized
	{
		get
		{
			return normalizedField;
		}
		set
		{
			normalizedField = value;
		}
	}
}

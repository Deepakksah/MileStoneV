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
public class PTZArgs
{
	private PTZPairDouble[] movementField;

	private PTZPairDouble[] speedField;

	private PTZPairBoolean[] automaticField;

	private bool normalizedField;

	[XmlElement("movement")]
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

	[XmlElement("speed")]
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

	[XmlElement("automatic")]
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

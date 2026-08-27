using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class HemisphereInfo : IHemisphereInfo
{
	private double radiusXField;

	private double radiusYField;

	private double centerXField;

	private double centerYField;

	public double RadiusX
	{
		get
		{
			return radiusXField;
		}
		set
		{
			radiusXField = value;
		}
	}

	public double RadiusY
	{
		get
		{
			return radiusYField;
		}
		set
		{
			radiusYField = value;
		}
	}

	public double CenterX
	{
		get
		{
			return centerXField;
		}
		set
		{
			centerXField = value;
		}
	}

	public double CenterY
	{
		get
		{
			return centerYField;
		}
		set
		{
			centerYField = value;
		}
	}
}

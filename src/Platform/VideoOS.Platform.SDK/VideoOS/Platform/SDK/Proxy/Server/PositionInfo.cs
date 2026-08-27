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
public class PositionInfo : IPositionInfo
{
	private double panField;

	private double tiltField;

	private double zoomField;

	public double Pan
	{
		get
		{
			return panField;
		}
		set
		{
			panField = value;
		}
	}

	public double Tilt
	{
		get
		{
			return tiltField;
		}
		set
		{
			tiltField = value;
		}
	}

	public double Zoom
	{
		get
		{
			return zoomField;
		}
		set
		{
			zoomField = value;
		}
	}
}

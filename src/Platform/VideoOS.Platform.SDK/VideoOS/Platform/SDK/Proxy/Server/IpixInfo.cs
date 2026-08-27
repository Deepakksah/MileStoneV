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
public class IpixInfo : IIpixInfo
{
	private bool ipixEnabledField;

	private HemisphereInfo hemisphereField;

	private PositionInfo homepositionField;

	private bool ceilingMountedField;

	IHemisphereInfo IIpixInfo.Hemisphere
	{
		get
		{
			return Hemisphere;
		}
		set
		{
			Hemisphere = (HemisphereInfo)value;
		}
	}

	IPositionInfo IIpixInfo.Homeposition
	{
		get
		{
			return Homeposition;
		}
		set
		{
			Homeposition = (PositionInfo)value;
		}
	}

	public bool IpixEnabled
	{
		get
		{
			return ipixEnabledField;
		}
		set
		{
			ipixEnabledField = value;
		}
	}

	public HemisphereInfo Hemisphere
	{
		get
		{
			return hemisphereField;
		}
		set
		{
			hemisphereField = value;
		}
	}

	public PositionInfo Homeposition
	{
		get
		{
			return homepositionField;
		}
		set
		{
			homepositionField = value;
		}
	}

	public bool CeilingMounted
	{
		get
		{
			return ceilingMountedField;
		}
		set
		{
			ceilingMountedField = value;
		}
	}
}

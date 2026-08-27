using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "HemisphereInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class HemisphereInfo : IHemisphereInfo
{
	private double CenterXField;

	private double CenterYField;

	private double RadiusXField;

	private double RadiusYField;

	[DataMember]
	public double CenterX
	{
		get
		{
			return CenterXField;
		}
		set
		{
			CenterXField = value;
		}
	}

	[DataMember]
	public double CenterY
	{
		get
		{
			return CenterYField;
		}
		set
		{
			CenterYField = value;
		}
	}

	[DataMember]
	public double RadiusX
	{
		get
		{
			return RadiusXField;
		}
		set
		{
			RadiusXField = value;
		}
	}

	[DataMember]
	public double RadiusY
	{
		get
		{
			return RadiusYField;
		}
		set
		{
			RadiusYField = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "PositionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class PositionInfo : IPositionInfo
{
	private double PanField;

	private double TiltField;

	private double ZoomField;

	[DataMember]
	public double Pan
	{
		get
		{
			return PanField;
		}
		set
		{
			PanField = value;
		}
	}

	[DataMember]
	public double Tilt
	{
		get
		{
			return TiltField;
		}
		set
		{
			TiltField = value;
		}
	}

	[DataMember]
	public double Zoom
	{
		get
		{
			return ZoomField;
		}
		set
		{
			ZoomField = value;
		}
	}
}

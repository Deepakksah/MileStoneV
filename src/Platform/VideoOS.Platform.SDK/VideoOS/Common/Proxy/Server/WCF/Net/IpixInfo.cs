using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "IpixInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class IpixInfo : IIpixInfo
{
	private bool CeilingMountedField;

	private HemisphereInfo HemisphereField;

	private PositionInfo HomepositionField;

	private bool IpixEnabledField;

	[DataMember]
	public bool CeilingMounted
	{
		get
		{
			return CeilingMountedField;
		}
		set
		{
			CeilingMountedField = value;
		}
	}

	[DataMember]
	public HemisphereInfo Hemisphere
	{
		get
		{
			return HemisphereField;
		}
		set
		{
			HemisphereField = value;
		}
	}

	[DataMember]
	public PositionInfo Homeposition
	{
		get
		{
			return HomepositionField;
		}
		set
		{
			HomepositionField = value;
		}
	}

	[DataMember]
	public bool IpixEnabled
	{
		get
		{
			return IpixEnabledField;
		}
		set
		{
			IpixEnabledField = value;
		}
	}

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
}

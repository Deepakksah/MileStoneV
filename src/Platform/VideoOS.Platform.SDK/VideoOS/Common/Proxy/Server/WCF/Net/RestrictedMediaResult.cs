using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "RestrictedMediaResult", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaResult : IRestrictedMediaResult
{
	private FaultDevice[] FaultDevicesField;

	private RestrictedMedia RestrictedMediaField;

	private VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus StatusField;

	private WarningDevice[] WarningDevicesField;

	[DataMember]
	public RestrictedMedia RestrictedMedia
	{
		get
		{
			return RestrictedMediaField;
		}
		set
		{
			RestrictedMediaField = value;
		}
	}

	[DataMember]
	public FaultDevice[] FaultDevices
	{
		get
		{
			return FaultDevicesField;
		}
		set
		{
			FaultDevicesField = value;
		}
	}

	[DataMember]
	public VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus Status
	{
		get
		{
			return StatusField;
		}
		set
		{
			StatusField = value;
		}
	}

	[DataMember]
	public WarningDevice[] WarningDevices
	{
		get
		{
			return WarningDevicesField;
		}
		set
		{
			WarningDevicesField = value;
		}
	}

	IRestrictedMedia IRestrictedMediaResult.RestrictedMedia
	{
		get
		{
			return RestrictedMedia;
		}
		set
		{
			RestrictedMedia = (RestrictedMedia)value;
		}
	}

	IFaultDevice[] IRestrictedMediaResult.FaultDevices
	{
		get
		{
			return FaultDevices;
		}
		set
		{
			FaultDevices = (FaultDevice[])value;
		}
	}

	IWarningDevice[] IRestrictedMediaResult.WarningDevices
	{
		get
		{
			return WarningDevices;
		}
		set
		{
			WarningDevices = (WarningDevice[])value;
		}
	}
}

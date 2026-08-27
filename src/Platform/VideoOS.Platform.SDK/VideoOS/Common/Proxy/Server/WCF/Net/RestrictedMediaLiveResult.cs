using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "RestrictedMediaLiveResult", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaLiveResult : IRestrictedMediaLiveResult
{
	private FaultDevice[] FaultDevicesField;

	private RestrictedMediaLive[] RestrictedMediaField;

	private VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus StatusField;

	private WarningDevice[] WarningDevicesField;

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
	public RestrictedMediaLive[] RestrictedMedia
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

	IRestrictedMediaLive[] IRestrictedMediaLiveResult.RestrictedMedia
	{
		get
		{
			return RestrictedMedia;
		}
		set
		{
			RestrictedMedia = (RestrictedMediaLive[])value;
		}
	}

	IFaultDevice[] IRestrictedMediaLiveResult.FaultDevices
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

	IWarningDevice[] IRestrictedMediaLiveResult.WarningDevices
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

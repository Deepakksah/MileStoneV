using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RestrictedMediaResult", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaResult : IExtensibleDataObject, IRestrictedMediaResult
{
	private ExtensionDataObject extensionDataField;

	private FaultDevice[] FaultDevicesField;

	private RestrictedMedia RestrictedMediaField;

	private RestrictedMediaResultStatus StatusField;

	private WarningDevice[] WarningDevicesField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
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
	public VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus Status
	{
		get
		{
			return (VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus)StatusField;
		}
		set
		{
			StatusField = (RestrictedMediaResultStatus)value;
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

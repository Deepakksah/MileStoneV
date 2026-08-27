using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RestrictedMediaResult", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RestrictedMediaResult : IExtensibleDataObject
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
	public RestrictedMediaResultStatus Status
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
}

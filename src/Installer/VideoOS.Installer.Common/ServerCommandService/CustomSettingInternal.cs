using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternal : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private CustomSettingInternalData CustomSettingDataField;

	private CustomSettingInfo CustomSettingInfoField;

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
	public CustomSettingInternalData CustomSettingData
	{
		get
		{
			return CustomSettingDataField;
		}
		set
		{
			CustomSettingDataField = value;
		}
	}

	[DataMember]
	public CustomSettingInfo CustomSettingInfo
	{
		get
		{
			return CustomSettingInfoField;
		}
		set
		{
			CustomSettingInfoField = value;
		}
	}
}

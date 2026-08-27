using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CustomSettingInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
[ExcludeFromCodeCoverage]
public class CustomSettingInternal : IExtensibleDataObject, ICustomSetting
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

	ICustomSettingInfo ICustomSetting.CustomSettingInfo
	{
		get
		{
			return CustomSettingInfo;
		}
		set
		{
			CustomSettingInfo = (CustomSettingInfo)value;
		}
	}

	ICustomSettingData ICustomSetting.CustomSettingData
	{
		get
		{
			return CustomSettingData;
		}
		set
		{
			CustomSettingData = (CustomSettingInternalData)value;
		}
	}
}

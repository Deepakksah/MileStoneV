using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "CustomSettingInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternal : ICustomSetting
{
	private CustomSettingInternalData CustomSettingDataField;

	private CustomSettingInfo CustomSettingInfoField;

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

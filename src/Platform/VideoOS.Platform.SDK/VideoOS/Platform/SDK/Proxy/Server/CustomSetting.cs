using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class CustomSetting : ICustomSetting
{
	private CustomSettingInfo customSettingInfoField;

	private CustomSettingData customSettingDataField;

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
			CustomSettingData = (CustomSettingData)value;
		}
	}

	public CustomSettingInfo CustomSettingInfo
	{
		get
		{
			return customSettingInfoField;
		}
		set
		{
			customSettingInfoField = value;
		}
	}

	public CustomSettingData CustomSettingData
	{
		get
		{
			return customSettingDataField;
		}
		set
		{
			customSettingDataField = value;
		}
	}
}

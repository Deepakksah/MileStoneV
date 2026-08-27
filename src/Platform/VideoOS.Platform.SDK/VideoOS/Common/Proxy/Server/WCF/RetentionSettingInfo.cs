using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RetentionSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class RetentionSettingInfo : IExtensibleDataObject, IRetentionSettingInfo
{
	private ExtensionDataObject extensionDataField;

	private RetentionOption[] RetentionOptionsField;

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
	public RetentionOption[] RetentionOptions
	{
		get
		{
			return RetentionOptionsField;
		}
		set
		{
			RetentionOptionsField = value;
		}
	}

	IRetentionOption[] IRetentionSettingInfo.RetentionOptions
	{
		get
		{
			return RetentionOptions;
		}
		set
		{
			RetentionOptions = (RetentionOption[])value;
		}
	}
}

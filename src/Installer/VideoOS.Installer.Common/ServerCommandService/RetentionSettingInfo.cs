using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RetentionSettingInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RetentionSettingInfo : IExtensibleDataObject
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
}

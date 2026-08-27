using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RetentionOption", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RetentionOption : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private RetentionOptionType RetentionOptionTypeField;

	private int RetentionUnitsField;

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
	public RetentionOptionType RetentionOptionType
	{
		get
		{
			return RetentionOptionTypeField;
		}
		set
		{
			RetentionOptionTypeField = value;
		}
	}

	[DataMember]
	public int RetentionUnits
	{
		get
		{
			return RetentionUnitsField;
		}
		set
		{
			RetentionUnitsField = value;
		}
	}
}

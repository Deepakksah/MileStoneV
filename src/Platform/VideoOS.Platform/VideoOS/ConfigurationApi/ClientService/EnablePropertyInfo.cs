using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EnablePropertyInfo", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class EnablePropertyInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DisplayNameField;

	private bool EnabledField;

	private string TranslationIdField;

	private bool UIToFollowEnabledField;

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
	public string DisplayName
	{
		get
		{
			return DisplayNameField;
		}
		set
		{
			DisplayNameField = value;
		}
	}

	[DataMember]
	public bool Enabled
	{
		get
		{
			return EnabledField;
		}
		set
		{
			EnabledField = value;
		}
	}

	[DataMember]
	public string TranslationId
	{
		get
		{
			return TranslationIdField;
		}
		set
		{
			TranslationIdField = value;
		}
	}

	[DataMember]
	public bool UIToFollowEnabled
	{
		get
		{
			return UIToFollowEnabledField;
		}
		set
		{
			UIToFollowEnabledField = value;
		}
	}
}

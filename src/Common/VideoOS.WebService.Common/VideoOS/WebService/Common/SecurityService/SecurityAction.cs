using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SecurityAction", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class SecurityAction : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ActionIdField;

	private string DisplayNameField;

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
	public string ActionId
	{
		get
		{
			return ActionIdField;
		}
		set
		{
			ActionIdField = value;
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
}

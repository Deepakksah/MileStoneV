using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ActionDefinition", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
[DebuggerDisplay("{NameField}")]
public class ActionDefinition : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DisplayNameField;

	private string NameField;

	private string NamespaceIdField;

	private bool VisibleField;

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
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public string NamespaceId
	{
		get
		{
			return NamespaceIdField;
		}
		set
		{
			NamespaceIdField = value;
		}
	}

	[DataMember]
	public bool Visible
	{
		get
		{
			return VisibleField;
		}
		set
		{
			VisibleField = value;
		}
	}
}

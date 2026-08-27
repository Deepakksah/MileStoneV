using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Security;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SecurityNamespaceInfo", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Server")]
public class SecurityNamespaceInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private ActionDefinition[] ActionsField;

	private string DisplayNameField;

	private string NameField;

	private string NamespaceIdField;

	private char SeparatorField;

	private int StructureField;

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
	public ActionDefinition[] Actions
	{
		get
		{
			return ActionsField;
		}
		set
		{
			ActionsField = value;
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
	public char Separator
	{
		get
		{
			return SeparatorField;
		}
		set
		{
			SeparatorField = value;
		}
	}

	[DataMember]
	public int Structure
	{
		get
		{
			return StructureField;
		}
		set
		{
			StructureField = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TypeForTree", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class TypeForTree : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private TypeID IDField;

	private bool IsCyclicField;

	private string NameField;

	private TypeID VisualOwnerIDField;

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
	public TypeID ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
		}
	}

	[DataMember]
	public bool IsCyclic
	{
		get
		{
			return IsCyclicField;
		}
		set
		{
			IsCyclicField = value;
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
	public TypeID VisualOwnerID
	{
		get
		{
			return VisualOwnerIDField;
		}
		set
		{
			VisualOwnerIDField = value;
		}
	}
}

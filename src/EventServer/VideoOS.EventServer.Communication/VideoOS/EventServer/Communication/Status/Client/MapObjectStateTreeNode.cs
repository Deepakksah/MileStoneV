using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "MapObjectStateTreeNode", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class MapObjectStateTreeNode : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private MapObjectState ObjectStateField;

	private string OwnerIDField;

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
	public MapObjectState ObjectState
	{
		get
		{
			return ObjectStateField;
		}
		set
		{
			ObjectStateField = value;
		}
	}

	[DataMember]
	public string OwnerID
	{
		get
		{
			return OwnerIDField;
		}
		set
		{
			OwnerIDField = value;
		}
	}
}

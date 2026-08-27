using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "VisualElementChange", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class VisualElementChange : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool DeletedField;

	private VisualElement VisualElementField;

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
	public bool Deleted
	{
		get
		{
			return DeletedField;
		}
		set
		{
			DeletedField = value;
		}
	}

	[DataMember]
	public VisualElement VisualElement
	{
		get
		{
			return VisualElementField;
		}
		set
		{
			VisualElementField = value;
		}
	}
}

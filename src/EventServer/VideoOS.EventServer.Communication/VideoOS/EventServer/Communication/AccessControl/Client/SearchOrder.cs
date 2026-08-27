using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchOrder", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class SearchOrder : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Direction DirectionField;

	private string TargetField;

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
	public Direction Direction
	{
		get
		{
			return DirectionField;
		}
		set
		{
			DirectionField = value;
		}
	}

	[DataMember]
	public string Target
	{
		get
		{
			return TargetField;
		}
		set
		{
			TargetField = value;
		}
	}
}

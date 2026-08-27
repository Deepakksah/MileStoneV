using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternal : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private ViewGroupInternalData ViewGroupDataField;

	private ViewGroupInfo ViewGroupInfoField;

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
	public ViewGroupInternalData ViewGroupData
	{
		get
		{
			return ViewGroupDataField;
		}
		set
		{
			ViewGroupDataField = value;
		}
	}

	[DataMember]
	public ViewGroupInfo ViewGroupInfo
	{
		get
		{
			return ViewGroupInfoField;
		}
		set
		{
			ViewGroupInfoField = value;
		}
	}
}

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "OrderBy", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class OrderBy : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Order OrderField;

	private Target TargetField;

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
	public Order Order
	{
		get
		{
			return OrderField;
		}
		set
		{
			OrderField = value;
		}
	}

	[DataMember]
	public Target Target
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

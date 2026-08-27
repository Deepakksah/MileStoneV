using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchOrders", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class SearchOrders : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private SearchOrder[] OrdersField;

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
	public SearchOrder[] Orders
	{
		get
		{
			return OrdersField;
		}
		set
		{
			OrdersField = value;
		}
	}
}

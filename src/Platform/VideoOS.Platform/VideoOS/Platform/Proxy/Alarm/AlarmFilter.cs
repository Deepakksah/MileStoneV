using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class AlarmFilter : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Condition[] ConditionsField;

	private OrderBy[] OrdersField;

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
	public Condition[] Conditions
	{
		get
		{
			return ConditionsField;
		}
		set
		{
			ConditionsField = value;
		}
	}

	[DataMember]
	public OrderBy[] Orders
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

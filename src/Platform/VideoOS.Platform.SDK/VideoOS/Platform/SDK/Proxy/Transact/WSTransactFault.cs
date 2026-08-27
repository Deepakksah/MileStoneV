using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Transact;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "WSTransactFault", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
public class WSTransactFault : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DataField;

	private WSTransactFaultState StateField;

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
	public string Data
	{
		get
		{
			return DataField;
		}
		set
		{
			DataField = value;
		}
	}

	[DataMember]
	public WSTransactFaultState State
	{
		get
		{
			return StateField;
		}
		set
		{
			StateField = value;
		}
	}
}

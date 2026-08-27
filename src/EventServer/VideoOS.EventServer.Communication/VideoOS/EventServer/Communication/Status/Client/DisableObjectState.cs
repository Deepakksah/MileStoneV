using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Status.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "DisableObjectState", Namespace = "http://videoos.net/2/MapServerMapCommand")]
public class DisableObjectState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DisableAlarmItem[] ItemsField;

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
	public DisableAlarmItem[] Items
	{
		get
		{
			return ItemsField;
		}
		set
		{
			ItemsField = value;
		}
	}
}

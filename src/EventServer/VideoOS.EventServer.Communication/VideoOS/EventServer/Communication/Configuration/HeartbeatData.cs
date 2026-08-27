using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "HeartbeatData", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class HeartbeatData : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DataField;

	private string TypeIdField;

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
	public string TypeId
	{
		get
		{
			return TypeIdField;
		}
		set
		{
			TypeIdField = value;
		}
	}
}

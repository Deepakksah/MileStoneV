using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AttachAndConnectionState", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class AttachAndConnectionState : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AttachStateField;

	private string ConnectionStateField;

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

	[DataMember(EmitDefaultValue = false)]
	public string AttachState
	{
		get
		{
			return AttachStateField;
		}
		set
		{
			AttachStateField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string ConnectionState
	{
		get
		{
			return ConnectionStateField;
		}
		set
		{
			ConnectionStateField = value;
		}
	}
}

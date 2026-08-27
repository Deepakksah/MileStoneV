using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "FaultDevice", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class FaultDevice : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid DeviceIdField;

	private int ErrorCodeField;

	private string MessageField;

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
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember]
	public int ErrorCode
	{
		get
		{
			return ErrorCodeField;
		}
		set
		{
			ErrorCodeField = value;
		}
	}

	[DataMember]
	public string Message
	{
		get
		{
			return MessageField;
		}
		set
		{
			MessageField = value;
		}
	}
}

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EventType", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class EventType : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string AutoCloseMessageField;

	private Guid DefaultSourceKindField;

	private Guid GroupIDField;

	private Guid IDField;

	private string MessageField;

	private TargetServerType ServerTypeField;

	private Guid[] SourceKindsField;

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
	public string AutoCloseMessage
	{
		get
		{
			return AutoCloseMessageField;
		}
		set
		{
			AutoCloseMessageField = value;
		}
	}

	[DataMember]
	public Guid DefaultSourceKind
	{
		get
		{
			return DefaultSourceKindField;
		}
		set
		{
			DefaultSourceKindField = value;
		}
	}

	[DataMember]
	public Guid GroupID
	{
		get
		{
			return GroupIDField;
		}
		set
		{
			GroupIDField = value;
		}
	}

	[DataMember]
	public Guid ID
	{
		get
		{
			return IDField;
		}
		set
		{
			IDField = value;
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

	[DataMember]
	public TargetServerType ServerType
	{
		get
		{
			return ServerTypeField;
		}
		set
		{
			ServerTypeField = value;
		}
	}

	[DataMember]
	public Guid[] SourceKinds
	{
		get
		{
			return SourceKindsField;
		}
		set
		{
			SourceKindsField = value;
		}
	}
}

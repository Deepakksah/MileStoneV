using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Event.Server.WebService.Action.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ActionExecutionParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.EventServer.WebService.Action")]
public class ActionExecutionParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private Guid ActionIdField;

	private Guid[] ActionTargetsField;

	private Guid[] ActionTargets2Field;

	private TriggerEventData TriggerEventDataField;

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
	public Guid ActionId
	{
		get
		{
			return ActionIdField;
		}
		set
		{
			ActionIdField = value;
		}
	}

	[DataMember]
	public Guid[] ActionTargets
	{
		get
		{
			return ActionTargetsField;
		}
		set
		{
			ActionTargetsField = value;
		}
	}

	[DataMember]
	public Guid[] ActionTargets2
	{
		get
		{
			return ActionTargets2Field;
		}
		set
		{
			ActionTargets2Field = value;
		}
	}

	[DataMember]
	public TriggerEventData TriggerEventData
	{
		get
		{
			return TriggerEventDataField;
		}
		set
		{
			TriggerEventDataField = value;
		}
	}
}

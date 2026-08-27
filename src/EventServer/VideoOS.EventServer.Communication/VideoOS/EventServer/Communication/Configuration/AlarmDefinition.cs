using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmDefinition", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public class AlarmDefinition : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AlarmAction[] AlarmActionsField;

	private bool AssignableToAdminsField;

	private bool AutoCloseField;

	private Guid CategoryField;

	private string DescriptionField;

	private FQID[] DisablingEventsField;

	private EnableRule EnableRuleField;

	private bool EnabledField;

	private FQID[] EnablingEventsField;

	private string EventTypeField;

	private Guid IdField;

	private FQID[] ManagementTimeoutEventsField;

	private TimeSpan ManagementTimeoutTimeField;

	private string NameField;

	private string OwnerField;

	private Guid PriorityField;

	private FQID[] RelatedCamerasField;

	private FQID[] SourcesField;

	private Guid TimeProfileIdField;

	private FQID[] TriggerEventsField;

	private int _mapType;

	[DataMember]
	public int MapType
	{
		get
		{
			return _mapType;
		}
		set
		{
			_mapType = value;
		}
	}

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
	public AlarmAction[] AlarmActions
	{
		get
		{
			return AlarmActionsField;
		}
		set
		{
			AlarmActionsField = value;
		}
	}

	[DataMember]
	public bool AssignableToAdmins
	{
		get
		{
			return AssignableToAdminsField;
		}
		set
		{
			AssignableToAdminsField = value;
		}
	}

	[DataMember]
	public bool AutoClose
	{
		get
		{
			return AutoCloseField;
		}
		set
		{
			AutoCloseField = value;
		}
	}

	[DataMember]
	public Guid Category
	{
		get
		{
			return CategoryField;
		}
		set
		{
			CategoryField = value;
		}
	}

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public FQID[] DisablingEvents
	{
		get
		{
			return DisablingEventsField;
		}
		set
		{
			DisablingEventsField = value;
		}
	}

	[DataMember]
	public EnableRule EnableRule
	{
		get
		{
			return EnableRuleField;
		}
		set
		{
			EnableRuleField = value;
		}
	}

	[DataMember]
	public bool Enabled
	{
		get
		{
			return EnabledField;
		}
		set
		{
			EnabledField = value;
		}
	}

	[DataMember]
	public FQID[] EnablingEvents
	{
		get
		{
			return EnablingEventsField;
		}
		set
		{
			EnablingEventsField = value;
		}
	}

	[DataMember]
	public string EventType
	{
		get
		{
			return EventTypeField;
		}
		set
		{
			EventTypeField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public FQID[] ManagementTimeoutEvents
	{
		get
		{
			return ManagementTimeoutEventsField;
		}
		set
		{
			ManagementTimeoutEventsField = value;
		}
	}

	[DataMember]
	public TimeSpan ManagementTimeoutTime
	{
		get
		{
			return ManagementTimeoutTimeField;
		}
		set
		{
			ManagementTimeoutTimeField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public string Owner
	{
		get
		{
			return OwnerField;
		}
		set
		{
			OwnerField = value;
		}
	}

	[DataMember]
	public Guid Priority
	{
		get
		{
			return PriorityField;
		}
		set
		{
			PriorityField = value;
		}
	}

	[DataMember]
	public FQID[] RelatedCameras
	{
		get
		{
			return RelatedCamerasField;
		}
		set
		{
			RelatedCamerasField = value;
		}
	}

	[DataMember]
	public FQID[] Sources
	{
		get
		{
			return SourcesField;
		}
		set
		{
			SourcesField = value;
		}
	}

	[DataMember]
	public Guid TimeProfileId
	{
		get
		{
			return TimeProfileIdField;
		}
		set
		{
			TimeProfileIdField = value;
		}
	}

	[DataMember]
	public FQID[] TriggerEvents
	{
		get
		{
			return TriggerEventsField;
		}
		set
		{
			TriggerEventsField = value;
		}
	}
}

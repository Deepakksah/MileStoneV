using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ConfigurationInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ConfigurationInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private AlertTypeGroupInfo[] AlertTypeGroupsField;

	private AlertTypeInfo[] AlertTypesField;

	private ApplicationSecurityInfo ApplicationAccessField;

	private AudioMessageInfo[] AudioMessagesField;

	private BookmarkSettingInfo BookmarkSettingsField;

	private CameraGroupInfo[] CameraGroupsField;

	private DeviceMappingInfo[] DeviceMappingsField;

	private EventTypeGroupInfo[] EventTypeGroupsField;

	private EventTypeInfo[] EventTypesField;

	private int FailoverCheckIntervalField;

	private InputGroupInfo[] InputGroupsField;

	private LicenseInfo[] LicensesField;

	private MatrixMonitorInfo[] MatrixMonitorsField;

	private MetadataDeviceGroupInfo[] MetadataDeviceGroupsField;

	private MicrophoneGroupInfo[] MicrophoneGroupsField;

	private OutputGroupInfo[] OutputGroupsField;

	private RecorderInfo[] RecordersField;

	private RetentionSettingInfo RetentionSettingsField;

	private string ServerDescriptionField;

	private Guid ServerIdField;

	private string ServerNameField;

	private ServerOption[] ServerOptionsField;

	private SpeakerGroupInfo[] SpeakerGroupsField;

	private SystemEventTypeInfo[] SystemEventTypesField;

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
	public AlertTypeGroupInfo[] AlertTypeGroups
	{
		get
		{
			return AlertTypeGroupsField;
		}
		set
		{
			AlertTypeGroupsField = value;
		}
	}

	[DataMember]
	public AlertTypeInfo[] AlertTypes
	{
		get
		{
			return AlertTypesField;
		}
		set
		{
			AlertTypesField = value;
		}
	}

	[DataMember]
	public ApplicationSecurityInfo ApplicationAccess
	{
		get
		{
			return ApplicationAccessField;
		}
		set
		{
			ApplicationAccessField = value;
		}
	}

	[DataMember]
	public AudioMessageInfo[] AudioMessages
	{
		get
		{
			return AudioMessagesField;
		}
		set
		{
			AudioMessagesField = value;
		}
	}

	[DataMember]
	public BookmarkSettingInfo BookmarkSettings
	{
		get
		{
			return BookmarkSettingsField;
		}
		set
		{
			BookmarkSettingsField = value;
		}
	}

	[DataMember]
	public CameraGroupInfo[] CameraGroups
	{
		get
		{
			return CameraGroupsField;
		}
		set
		{
			CameraGroupsField = value;
		}
	}

	[DataMember]
	public DeviceMappingInfo[] DeviceMappings
	{
		get
		{
			return DeviceMappingsField;
		}
		set
		{
			DeviceMappingsField = value;
		}
	}

	[DataMember]
	public EventTypeGroupInfo[] EventTypeGroups
	{
		get
		{
			return EventTypeGroupsField;
		}
		set
		{
			EventTypeGroupsField = value;
		}
	}

	[DataMember]
	public EventTypeInfo[] EventTypes
	{
		get
		{
			return EventTypesField;
		}
		set
		{
			EventTypesField = value;
		}
	}

	[DataMember]
	public int FailoverCheckInterval
	{
		get
		{
			return FailoverCheckIntervalField;
		}
		set
		{
			FailoverCheckIntervalField = value;
		}
	}

	[DataMember]
	public InputGroupInfo[] InputGroups
	{
		get
		{
			return InputGroupsField;
		}
		set
		{
			InputGroupsField = value;
		}
	}

	[DataMember]
	public LicenseInfo[] Licenses
	{
		get
		{
			return LicensesField;
		}
		set
		{
			LicensesField = value;
		}
	}

	[DataMember]
	public MatrixMonitorInfo[] MatrixMonitors
	{
		get
		{
			return MatrixMonitorsField;
		}
		set
		{
			MatrixMonitorsField = value;
		}
	}

	[DataMember]
	public MetadataDeviceGroupInfo[] MetadataDeviceGroups
	{
		get
		{
			return MetadataDeviceGroupsField;
		}
		set
		{
			MetadataDeviceGroupsField = value;
		}
	}

	[DataMember]
	public MicrophoneGroupInfo[] MicrophoneGroups
	{
		get
		{
			return MicrophoneGroupsField;
		}
		set
		{
			MicrophoneGroupsField = value;
		}
	}

	[DataMember]
	public OutputGroupInfo[] OutputGroups
	{
		get
		{
			return OutputGroupsField;
		}
		set
		{
			OutputGroupsField = value;
		}
	}

	[DataMember]
	public RecorderInfo[] Recorders
	{
		get
		{
			return RecordersField;
		}
		set
		{
			RecordersField = value;
		}
	}

	[DataMember]
	public RetentionSettingInfo RetentionSettings
	{
		get
		{
			return RetentionSettingsField;
		}
		set
		{
			RetentionSettingsField = value;
		}
	}

	[DataMember]
	public string ServerDescription
	{
		get
		{
			return ServerDescriptionField;
		}
		set
		{
			ServerDescriptionField = value;
		}
	}

	[DataMember]
	public Guid ServerId
	{
		get
		{
			return ServerIdField;
		}
		set
		{
			ServerIdField = value;
		}
	}

	[DataMember]
	public string ServerName
	{
		get
		{
			return ServerNameField;
		}
		set
		{
			ServerNameField = value;
		}
	}

	[DataMember]
	public ServerOption[] ServerOptions
	{
		get
		{
			return ServerOptionsField;
		}
		set
		{
			ServerOptionsField = value;
		}
	}

	[DataMember]
	public SpeakerGroupInfo[] SpeakerGroups
	{
		get
		{
			return SpeakerGroupsField;
		}
		set
		{
			SpeakerGroupsField = value;
		}
	}

	[DataMember]
	public SystemEventTypeInfo[] SystemEventTypes
	{
		get
		{
			return SystemEventTypesField;
		}
		set
		{
			SystemEventTypesField = value;
		}
	}
}

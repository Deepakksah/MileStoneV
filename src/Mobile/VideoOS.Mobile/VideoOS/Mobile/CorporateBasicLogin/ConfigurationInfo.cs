using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ConfigurationInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ConfigurationInfo : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private AlertTypeGroupInfo[] AlertTypeGroupsField;

	[OptionalField]
	private AlertTypeInfo[] AlertTypesField;

	[OptionalField]
	private ApplicationSecurityInfo ApplicationAccessField;

	[OptionalField]
	private BookmarkSettingInfo BookmarkSettingsField;

	[OptionalField]
	private CameraGroupInfo[] CameraGroupsField;

	[OptionalField]
	private DeviceMappingInfo[] DeviceMappingsField;

	[OptionalField]
	private EventTypeGroupInfo[] EventTypeGroupsField;

	[OptionalField]
	private EventTypeInfo[] EventTypesField;

	[OptionalField]
	private int FailoverCheckIntervalField;

	[OptionalField]
	private InputGroupInfo[] InputGroupsField;

	[OptionalField]
	private LicenseInfo[] LicensesField;

	[OptionalField]
	private MatrixMonitorInfo[] MatrixMonitorsField;

	[OptionalField]
	private MicrophoneGroupInfo[] MicrophoneGroupsField;

	[OptionalField]
	private OutputGroupInfo[] OutputGroupsField;

	[OptionalField]
	private RecorderInfo[] RecordersField;

	[OptionalField]
	private string ServerDescriptionField;

	[OptionalField]
	private Guid ServerIdField;

	[OptionalField]
	private string ServerNameField;

	[OptionalField]
	private ServerOption[] ServerOptionsField;

	[OptionalField]
	private SpeakerGroupInfo[] SpeakerGroupsField;

	[OptionalField]
	private SystemEventTypeInfo[] SystemEventTypesField;

	[Browsable(false)]
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
			if (AlertTypeGroupsField != value)
			{
				AlertTypeGroupsField = value;
				RaisePropertyChanged("AlertTypeGroups");
			}
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
			if (AlertTypesField != value)
			{
				AlertTypesField = value;
				RaisePropertyChanged("AlertTypes");
			}
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
			if (ApplicationAccessField != value)
			{
				ApplicationAccessField = value;
				RaisePropertyChanged("ApplicationAccess");
			}
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
			if (BookmarkSettingsField != value)
			{
				BookmarkSettingsField = value;
				RaisePropertyChanged("BookmarkSettings");
			}
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
			if (CameraGroupsField != value)
			{
				CameraGroupsField = value;
				RaisePropertyChanged("CameraGroups");
			}
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
			if (DeviceMappingsField != value)
			{
				DeviceMappingsField = value;
				RaisePropertyChanged("DeviceMappings");
			}
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
			if (EventTypeGroupsField != value)
			{
				EventTypeGroupsField = value;
				RaisePropertyChanged("EventTypeGroups");
			}
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
			if (EventTypesField != value)
			{
				EventTypesField = value;
				RaisePropertyChanged("EventTypes");
			}
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
			if (!FailoverCheckIntervalField.Equals(value))
			{
				FailoverCheckIntervalField = value;
				RaisePropertyChanged("FailoverCheckInterval");
			}
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
			if (InputGroupsField != value)
			{
				InputGroupsField = value;
				RaisePropertyChanged("InputGroups");
			}
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
			if (LicensesField != value)
			{
				LicensesField = value;
				RaisePropertyChanged("Licenses");
			}
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
			if (MatrixMonitorsField != value)
			{
				MatrixMonitorsField = value;
				RaisePropertyChanged("MatrixMonitors");
			}
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
			if (MicrophoneGroupsField != value)
			{
				MicrophoneGroupsField = value;
				RaisePropertyChanged("MicrophoneGroups");
			}
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
			if (OutputGroupsField != value)
			{
				OutputGroupsField = value;
				RaisePropertyChanged("OutputGroups");
			}
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
			if (RecordersField != value)
			{
				RecordersField = value;
				RaisePropertyChanged("Recorders");
			}
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
			if ((object)ServerDescriptionField != value)
			{
				ServerDescriptionField = value;
				RaisePropertyChanged("ServerDescription");
			}
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
			if (!ServerIdField.Equals(value))
			{
				ServerIdField = value;
				RaisePropertyChanged("ServerId");
			}
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
			if ((object)ServerNameField != value)
			{
				ServerNameField = value;
				RaisePropertyChanged("ServerName");
			}
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
			if (ServerOptionsField != value)
			{
				ServerOptionsField = value;
				RaisePropertyChanged("ServerOptions");
			}
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
			if (SpeakerGroupsField != value)
			{
				SpeakerGroupsField = value;
				RaisePropertyChanged("SpeakerGroups");
			}
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
			if (SystemEventTypesField != value)
			{
				SystemEventTypesField = value;
				RaisePropertyChanged("SystemEventTypes");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ConfigurationInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ConfigurationInfo : IConfigurationInfo
{
	public AlertTypeGroupInfo[] AlertTypeGroupsField;

	public AlertTypeInfo[] AlertTypesField;

	public ApplicationSecurityInfo ApplicationAccessField;

	public AudioMessageInfo[] AudioMessagesField;

	public BookmarkSettingInfo BookmarkSettingsField;

	public CameraGroupInfo[] CameraGroupsField;

	public DeviceMappingInfo[] DeviceMappingsField;

	public EventTypeGroupInfo[] EventTypeGroupsField;

	public EventTypeInfo[] EventTypesField;

	public int FailoverCheckIntervalField;

	public InputGroupInfo[] InputGroupsField;

	public LicenseInfo[] LicensesField;

	public MatrixMonitorInfo[] MatrixMonitorsField;

	public MetadataDeviceGroupInfo[] MetadataDeviceGroupsField;

	public MicrophoneGroupInfo[] MicrophoneGroupsField;

	public OutputGroupInfo[] OutputGroupsField;

	public RecorderInfo[] RecordersField;

	public RetentionSettingInfo RetentionSettingsField;

	public string ServerDescriptionField;

	public Guid ServerIdField;

	public string ServerNameField;

	public ServerOption[] ServerOptionsField;

	public SpeakerGroupInfo[] SpeakerGroupsField;

	public SystemEventTypeInfo[] SystemEventTypesField;

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

	IRetentionSettingInfo IConfigurationInfo.RetentionSettings
	{
		get
		{
			return RetentionSettings;
		}
		set
		{
			RetentionSettings = (RetentionSettingInfo)value;
		}
	}

	IServerOption[] IConfigurationInfo.ServerOptions
	{
		get
		{
			return ServerOptions;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("ServerOptions");
			}
			ServerOptions = (from i in value.OfType<ServerOption>()
				where i.IsValid()
				select i).ToArray();
		}
	}

	IRecorderInfo[] IConfigurationInfo.Recorders
	{
		get
		{
			return Recorders;
		}
		set
		{
			Recorders = (RecorderInfo[])value;
		}
	}

	IApplicationSecurityInfo IConfigurationInfo.ApplicationAccess
	{
		get
		{
			return ApplicationAccess;
		}
		set
		{
			ApplicationAccess = (ApplicationSecurityInfo)value;
		}
	}

	ILicenseInfo[] IConfigurationInfo.Licenses
	{
		get
		{
			return Licenses;
		}
		set
		{
			Licenses = (LicenseInfo[])value;
		}
	}

	IDeviceMappingInfo[] IConfigurationInfo.DeviceMappings
	{
		get
		{
			return DeviceMappings;
		}
		set
		{
			DeviceMappings = (DeviceMappingInfo[])value;
		}
	}

	ICameraGroupInfo[] IConfigurationInfo.CameraGroups
	{
		get
		{
			return CameraGroups;
		}
		set
		{
			CameraGroups = (CameraGroupInfo[])value;
		}
	}

	IInputGroupInfo[] IConfigurationInfo.InputGroups
	{
		get
		{
			return InputGroups;
		}
		set
		{
			InputGroups = (InputGroupInfo[])value;
		}
	}

	IOutputGroupInfo[] IConfigurationInfo.OutputGroups
	{
		get
		{
			return OutputGroups;
		}
		set
		{
			OutputGroups = (OutputGroupInfo[])value;
		}
	}

	IMicrophoneGroupInfo[] IConfigurationInfo.MicrophoneGroups
	{
		get
		{
			return MicrophoneGroups;
		}
		set
		{
			MicrophoneGroups = (MicrophoneGroupInfo[])value;
		}
	}

	ISpeakerGroupInfo[] IConfigurationInfo.SpeakerGroups
	{
		get
		{
			return SpeakerGroups;
		}
		set
		{
			SpeakerGroups = (SpeakerGroupInfo[])value;
		}
	}

	IEventTypeGroupInfo[] IConfigurationInfo.EventTypeGroups
	{
		get
		{
			return EventTypeGroups;
		}
		set
		{
			EventTypeGroups = (EventTypeGroupInfo[])value;
		}
	}

	IEventTypeInfo[] IConfigurationInfo.EventTypes
	{
		get
		{
			return EventTypes;
		}
		set
		{
			EventTypes = (EventTypeInfo[])value;
		}
	}

	ISystemEventTypeInfo[] IConfigurationInfo.SystemEventTypes
	{
		get
		{
			return SystemEventTypes;
		}
		set
		{
			SystemEventTypes = (SystemEventTypeInfo[])value;
		}
	}

	IAlertTypeGroupInfo[] IConfigurationInfo.AlertTypeGroups
	{
		get
		{
			return AlertTypeGroups;
		}
		set
		{
			AlertTypeGroups = (AlertTypeGroupInfo[])value;
		}
	}

	IAlertTypeInfo[] IConfigurationInfo.AlertTypes
	{
		get
		{
			return AlertTypes;
		}
		set
		{
			AlertTypes = (AlertTypeInfo[])value;
		}
	}

	IMatrixMonitorInfo[] IConfigurationInfo.MatrixMonitors
	{
		get
		{
			return MatrixMonitors;
		}
		set
		{
			MatrixMonitors = (MatrixMonitorInfo[])value;
		}
	}

	IMetadataDeviceGroupInfo[] IConfigurationInfo.MetadataDeviceGroups
	{
		get
		{
			return MetadataDeviceGroups;
		}
		set
		{
			MetadataDeviceGroups = (MetadataDeviceGroupInfo[])value;
		}
	}

	IBookmarkSettingInfo IConfigurationInfo.BookmarkSettings
	{
		get
		{
			return BookmarkSettings;
		}
		set
		{
			BookmarkSettings = (BookmarkSettingInfo)value;
		}
	}
}

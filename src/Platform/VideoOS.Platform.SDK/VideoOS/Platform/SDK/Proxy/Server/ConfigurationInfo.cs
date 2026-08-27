using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ConfigurationInfo : IConfigurationInfo
{
	private Guid serverIdField;

	private string serverNameField;

	private string serverDescriptionField;

	private ServerOption[] serverOptionsField;

	private RecorderInfo[] recordersField;

	private ApplicationSecurityInfo applicationAccessField;

	private int failoverCheckIntervalField;

	private LicenseInfo[] licensesField;

	private DeviceMappingInfo[] deviceMappingsField;

	private CameraGroupInfo[] cameraGroupsField;

	private InputGroupInfo[] inputGroupsField;

	private OutputGroupInfo[] outputGroupsField;

	private MicrophoneGroupInfo[] microphoneGroupsField;

	private SpeakerGroupInfo[] speakerGroupsField;

	private EventTypeGroupInfo[] eventTypeGroupsField;

	private EventTypeInfo[] eventTypesField;

	private SystemEventTypeInfo[] systemEventTypesField;

	private AlertTypeGroupInfo[] alertTypeGroupsField;

	private AlertTypeInfo[] alertTypesField;

	private MatrixMonitorInfo[] matrixMonitorsField;

	private BookmarkSettingInfo bookmarkSettingsField;

	private RetentionSettingInfo retentionSettingsField;

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

	public MetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

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

	public Guid ServerId
	{
		get
		{
			return serverIdField;
		}
		set
		{
			serverIdField = value;
		}
	}

	public string ServerName
	{
		get
		{
			return serverNameField;
		}
		set
		{
			serverNameField = value;
		}
	}

	public string ServerDescription
	{
		get
		{
			return serverDescriptionField;
		}
		set
		{
			serverDescriptionField = value;
		}
	}

	public ServerOption[] ServerOptions
	{
		get
		{
			return serverOptionsField;
		}
		set
		{
			serverOptionsField = value;
		}
	}

	public RecorderInfo[] Recorders
	{
		get
		{
			return recordersField;
		}
		set
		{
			recordersField = value;
		}
	}

	public ApplicationSecurityInfo ApplicationAccess
	{
		get
		{
			return applicationAccessField;
		}
		set
		{
			applicationAccessField = value;
		}
	}

	public int FailoverCheckInterval
	{
		get
		{
			return failoverCheckIntervalField;
		}
		set
		{
			failoverCheckIntervalField = value;
		}
	}

	public LicenseInfo[] Licenses
	{
		get
		{
			return licensesField;
		}
		set
		{
			licensesField = value;
		}
	}

	public DeviceMappingInfo[] DeviceMappings
	{
		get
		{
			return deviceMappingsField;
		}
		set
		{
			deviceMappingsField = value;
		}
	}

	public CameraGroupInfo[] CameraGroups
	{
		get
		{
			return cameraGroupsField;
		}
		set
		{
			cameraGroupsField = value;
		}
	}

	public InputGroupInfo[] InputGroups
	{
		get
		{
			return inputGroupsField;
		}
		set
		{
			inputGroupsField = value;
		}
	}

	public OutputGroupInfo[] OutputGroups
	{
		get
		{
			return outputGroupsField;
		}
		set
		{
			outputGroupsField = value;
		}
	}

	public MicrophoneGroupInfo[] MicrophoneGroups
	{
		get
		{
			return microphoneGroupsField;
		}
		set
		{
			microphoneGroupsField = value;
		}
	}

	public SpeakerGroupInfo[] SpeakerGroups
	{
		get
		{
			return speakerGroupsField;
		}
		set
		{
			speakerGroupsField = value;
		}
	}

	public EventTypeGroupInfo[] EventTypeGroups
	{
		get
		{
			return eventTypeGroupsField;
		}
		set
		{
			eventTypeGroupsField = value;
		}
	}

	public EventTypeInfo[] EventTypes
	{
		get
		{
			return eventTypesField;
		}
		set
		{
			eventTypesField = value;
		}
	}

	public SystemEventTypeInfo[] SystemEventTypes
	{
		get
		{
			return systemEventTypesField;
		}
		set
		{
			systemEventTypesField = value;
		}
	}

	public AlertTypeGroupInfo[] AlertTypeGroups
	{
		get
		{
			return alertTypeGroupsField;
		}
		set
		{
			alertTypeGroupsField = value;
		}
	}

	public AlertTypeInfo[] AlertTypes
	{
		get
		{
			return alertTypesField;
		}
		set
		{
			alertTypesField = value;
		}
	}

	public MatrixMonitorInfo[] MatrixMonitors
	{
		get
		{
			return matrixMonitorsField;
		}
		set
		{
			matrixMonitorsField = value;
		}
	}

	public BookmarkSettingInfo BookmarkSettings
	{
		get
		{
			return bookmarkSettingsField;
		}
		set
		{
			bookmarkSettingsField = value;
		}
	}

	public RetentionSettingInfo RetentionSettings
	{
		get
		{
			return retentionSettingsField;
		}
		set
		{
			retentionSettingsField = value;
		}
	}
}

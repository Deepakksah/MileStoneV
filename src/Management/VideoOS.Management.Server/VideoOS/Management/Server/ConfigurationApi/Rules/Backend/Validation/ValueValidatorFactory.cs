using System;
using System.Collections.Generic;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;

public class ValueValidatorFactory : IValueValidatorFactory
{
	private readonly Dictionary<Type, IValueValidator> _validators;

	private readonly Dictionary<Tuple<EventGroupType, bool>, IValueValidator> _validatorsByEventGroupTypeAndIsGroup;

	public ValueValidatorFactory(IDeviceProxy deviceProxy, ITimeProfileProxy timeProfileProxy, IMIPObjectProxy mipObjectProxy, IMIPServerGroupProxy serverGroupProxy, IHardwareProxy hardwareProxy, IRecordingServerProxy recordingServerProxy, IServerInformationProxy serverInformationProxy, IVideoWallProxy videoWallProxy, IVideoWallPresetProxy videoWallPresetProxy, IMonitorProxy monitorProxy, ILayoutProxy layoutProxy, IArchiveProxy archiveProxy, IRecorderPluginProxy recorderPluginProxy, IDeviceSettingsProxy deviceSettingsProxy, ISiteProxy siteProxy, IUserDefinedEventProxy userDefinedEventProxy, IPatrollingProfileProxy patrollingProfileProxy)
	{
		_validators = new Dictionary<Type, IValueValidator>
		{
			{
				typeof(DeviceParameterValueValidator),
				new DeviceParameterValueValidator(deviceProxy, new DeviceFilterValidator())
			},
			{
				typeof(DeviceGroupParameterValueValidator),
				new DeviceGroupParameterValueValidator(deviceProxy)
			},
			{
				typeof(TimeProfileParameterValueValidator),
				new TimeProfileParameterValueValidator(timeProfileProxy)
			},
			{
				typeof(PtzPresetParameterValueValidator),
				new PtzPresetParameterValueValidator(deviceProxy)
			},
			{
				typeof(TimeParameterValueValidator),
				new TimeParameterValueValidator()
			},
			{
				typeof(AccessControlEventParameterValueValidator),
				new AccessControlEventParameterValueValidator(mipObjectProxy, serverGroupProxy)
			},
			{
				typeof(AccessControlGroupEventParameterValueValidator),
				new AccessControlGroupEventParameterValueValidator(serverGroupProxy)
			},
			{
				typeof(PluginItemParameterValueValidator),
				new PluginItemParameterValueValidator(mipObjectProxy)
			},
			{
				typeof(PluginGroupItemParameterValueValidator),
				new PluginGroupItemParameterValueValidator(serverGroupProxy)
			},
			{
				typeof(HardwareParameterValueValidator),
				new HardwareParameterValueValidator(hardwareProxy)
			},
			{
				typeof(RecordingServerParameterValueValidator),
				new RecordingServerParameterValueValidator(recordingServerProxy)
			},
			{
				typeof(ExternalParameterValueValidator),
				new ExternalParameterValueValidator()
			},
			{
				typeof(OtherParameterValueValidator),
				new OtherParameterValueValidator()
			},
			{
				typeof(SystemDashboardServerParameterValueValidator),
				new SystemDashboardServerParameterValueValidator(recordingServerProxy, serverInformationProxy)
			},
			{
				typeof(SystemDashboardRecordingServerParameterValueValidator),
				new SystemDashboardRecordingServerParameterValueValidator(recordingServerProxy)
			},
			{
				typeof(FramerateValueValidator),
				new FramerateValueValidator()
			},
			{
				typeof(VideoWallParameterValueValidator),
				new VideoWallParameterValueValidator(videoWallProxy)
			},
			{
				typeof(VideoWallPresetParameterValueValidator),
				new VideoWallPresetParameterValueValidator(videoWallPresetProxy)
			},
			{
				typeof(MonitorParameterValueValidator),
				new MonitorParameterValueValidator(monitorProxy)
			},
			{
				typeof(LayoutParameterValueValidator),
				new LayoutParameterValueValidator(layoutProxy)
			},
			{
				typeof(ArchiveParameterValueValidator),
				new ArchiveParameterValueValidator(archiveProxy)
			},
			{
				typeof(MessageTextParameterValueValidator),
				new MessageTextParameterValueValidator()
			},
			{
				typeof(RecorderPluginValueValidator),
				new RecorderPluginValueValidator(recorderPluginProxy)
			},
			{
				typeof(DeviceSettingsParameterValueValidator),
				new DeviceSettingsParameterValueValidator(deviceProxy, deviceSettingsProxy)
			},
			{
				typeof(SiteParameterValueValidator),
				new SiteParameterValueValidator(siteProxy)
			},
			{
				typeof(UserDefinedEventValueValidator),
				new UserDefinedEventValueValidator(userDefinedEventProxy, siteProxy)
			},
			{
				typeof(PatrollingProfileParameterValueValidator),
				new PatrollingProfileParameterValueValidator(patrollingProfileProxy, deviceProxy)
			},
			{
				typeof(MatrixParameterValueValidator),
				new MatrixParameterValueValidator()
			}
		};
		_validatorsByEventGroupTypeAndIsGroup = new Dictionary<Tuple<EventGroupType, bool>, IValueValidator>
		{
			{
				new Tuple<EventGroupType, bool>(EventGroupType.DevicePredefined, item2: true),
				_validators[typeof(DeviceGroupParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.DeviceConfigurable, item2: true),
				_validators[typeof(DeviceGroupParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.DevicePredefined, item2: false),
				_validators[typeof(DeviceParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.DeviceConfigurable, item2: false),
				_validators[typeof(DeviceParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AccessControlCategory, item2: false),
				_validators[typeof(AccessControlEventParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AccessControlEvent, item2: false),
				_validators[typeof(AccessControlEventParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AccessControlCategory, item2: true),
				_validators[typeof(AccessControlGroupEventParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AccessControlEvent, item2: true),
				_validators[typeof(AccessControlGroupEventParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AnalyticsEvent, item2: true),
				_validators[typeof(DeviceGroupParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.AnalyticsEvent, item2: false),
				_validators[typeof(DeviceParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.PluginEvent, item2: false),
				_validators[typeof(PluginItemParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.PluginEvent, item2: true),
				_validators[typeof(PluginGroupItemParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.HardwareConfigurable, item2: false),
				_validators[typeof(HardwareParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.HardwarePredefined, item2: false),
				_validators[typeof(HardwareParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.RecordingServers, item2: false),
				_validators[typeof(RecordingServerParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.GenericEvents, item2: false),
				_validators[typeof(ExternalParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.PredefinedEvents, item2: false),
				_validators[typeof(ExternalParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.UserDefinedEvents, item2: false),
				_validators[typeof(ExternalParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.Other, item2: false),
				_validators[typeof(OtherParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.SystemDashboardCamera, item2: true),
				_validators[typeof(DeviceGroupParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.SystemDashboardCamera, item2: false),
				_validators[typeof(DeviceParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.SystemDashboardServer, item2: false),
				_validators[typeof(SystemDashboardServerParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.SystemDashboardDisk, item2: false),
				_validators[typeof(SystemDashboardRecordingServerParameterValueValidator)]
			},
			{
				new Tuple<EventGroupType, bool>(EventGroupType.SystemDashboardStorage, item2: false),
				_validators[typeof(SystemDashboardRecordingServerParameterValueValidator)]
			}
		};
	}

	public IValueValidator GetValidator(Type validatorType)
	{
		if (!_validators.TryGetValue(validatorType, out var value))
		{
			return null;
		}
		return value;
	}

	public IValueValidator GetValidator(EventGroupType eventGroupType, bool isGroup)
	{
		if (!_validatorsByEventGroupTypeAndIsGroup.TryGetValue(new Tuple<EventGroupType, bool>(eventGroupType, isGroup), out var value))
		{
			return null;
		}
		return value;
	}
}

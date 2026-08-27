namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public enum EventGroupType
{
	None,
	NotSupported,
	DevicePredefined,
	DeviceConfigurable,
	AccessControlEvent,
	AccessControlCategory,
	AnalyticsEvent,
	PluginEvent,
	HardwarePredefined,
	HardwareConfigurable,
	RecordingServers,
	PredefinedEvents,
	GenericEvents,
	UserDefinedEvents,
	Other,
	SystemDashboardServer,
	SystemDashboardCamera,
	SystemDashboardDisk,
	SystemDashboardStorage
}

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RHSettingsDevice
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. Id: {1}; Name: {2}", "RHSettingValue", DeviceId, DeviceName));

	[DataMember]
	public Guid DeviceId { get; private set; }

	[DataMember]
	public string DeviceName { get; private set; }

	[DataMember]
	public RHSettingValue[] InheritedDeviceSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] DefaultDeviceSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] InheritedDeviceStreamSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] DefaultDeviceStreamSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] InheritedDeviceMetadataItemSettingValues { get; private set; }

	[DataMember]
	public RHSettingValue[] DefaultDeviceMetadataItemSettingValues { get; private set; }

	[DataMember]
	public string[] InheritedPatrollingProfiles { get; private set; }

	[DataMember]
	public string[] DeletedPatrollingProfiles { get; private set; }

	[DataMember]
	public RHEventValue[] InheritedEvents { get; private set; }

	[DataMember]
	public RHEventValue[] DeletedEvents { get; private set; }

	public RHSettingsDevice(Guid deviceId, string deviceName, RHSettingValue[] inheritedDeviceSettingValues, RHSettingValue[] defaultDeviceSettingValues, RHSettingValue[] inheritedDeviceStreamSettingValues, RHSettingValue[] defaultDeviceStreamSettingValues, RHSettingValue[] inheritedDeviceMetadataItemSettingValues, RHSettingValue[] defaultDeviceMetadataItemSettingValues, string[] inheritedPatrollingProfiles, string[] deletedPatrollingProfiles, RHEventValue[] inheritedEvents, RHEventValue[] deletedEvents)
	{
		DeviceId = deviceId;
		DeviceName = deviceName;
		InheritedDeviceSettingValues = inheritedDeviceSettingValues ?? new RHSettingValue[0];
		DefaultDeviceSettingValues = defaultDeviceSettingValues ?? new RHSettingValue[0];
		InheritedDeviceStreamSettingValues = inheritedDeviceStreamSettingValues ?? new RHSettingValue[0];
		DefaultDeviceStreamSettingValues = defaultDeviceStreamSettingValues ?? new RHSettingValue[0];
		InheritedDeviceMetadataItemSettingValues = inheritedDeviceMetadataItemSettingValues ?? new RHSettingValue[0];
		DefaultDeviceMetadataItemSettingValues = defaultDeviceMetadataItemSettingValues ?? new RHSettingValue[0];
		InheritedPatrollingProfiles = inheritedPatrollingProfiles ?? new string[0];
		DeletedPatrollingProfiles = deletedPatrollingProfiles ?? new string[0];
		InheritedEvents = inheritedEvents ?? new RHEventValue[0];
		DeletedEvents = deletedEvents ?? new RHEventValue[0];
	}
}

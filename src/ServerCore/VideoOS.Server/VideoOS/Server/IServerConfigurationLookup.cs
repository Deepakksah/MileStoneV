using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server;

public interface IServerConfigurationLookup
{
	bool IsRecorderRegistered(Guid recorderId);

	bool IsRecorderEnabled(Guid recorderId);

	bool RecorderExists(Guid recorderId);

	bool RecorderHasLockedDeviceDataDistribution(Guid recorderId);

	string FindRecorderDisplayName(Guid recorderId);

	Guid FindRecorderIdFromServiceId(Guid serviceId);

	string FindDeviceDisplayName(Guid deviceId);

	bool DeviceHasEvidenceLock(string deviceId);

	Guid QueryDeviceEdgeTrackIdByDeviceId(Guid deviceId);

	DeviceDataDistribution[] QueryDeviceDataDistribution(Guid deviceId, Guid recorderId);

	string FindSourceDisplayName(Guid sourceId, SourceType sourceType);

	SourceType FindEventSourceTypeFromEventTypeID(Guid eventTypeId);

	Subtype FindEventSourceSubtypeFromEventTypeID(Guid eventTypeId);

	string FindEventDisplayNameFromEventTypeID(Guid eventTypeId, string defaultValue);

	string FindAVICodec();

	IPHardware FindIPHardware(Guid hardwareId);

	Guid QuerydDefaultStorageIdByRecorder(Guid recorderId);
}

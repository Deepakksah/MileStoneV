using System;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.DataComponents;

public interface IConfigurationDataComponent : IDisposable
{
	Guid QueryServiceIdFromRecorderId(Guid recorderId, RecordingServerServiceType serviceType, bool activeServiceOnly);

	string QueryTemporaryLicenseStorageData();

	void WriteTemporaryLicenseStorageData(string data);

	Guid QueryOriginalIdByMacAddressDeviceIndex(string macAddress, int deviceIndex);

	string QueryUniqueIdByMacAddressAndOriginalId(string macAddress, string originalId);

	string QueryDefinitionDigestByDeviceId(Guid deviceId);

	Guid[] QueryDeviceIdsByRecorder(Guid recorderId);

	Guid QueryRecorderIdByDeviceId(Guid deviceId);

	string FindRoleNameByRoleId(Guid roleId);

	string FindDeviceNameByDeviceId(Guid deviceId);

	string FindMatrixNameByMatrixId(Guid matrixId);

	string FindDeviceTypeByDeviceId(Guid deviceId);

	string FindTimeProfileNameByTimeProfileId(Guid timeProfileId);
}

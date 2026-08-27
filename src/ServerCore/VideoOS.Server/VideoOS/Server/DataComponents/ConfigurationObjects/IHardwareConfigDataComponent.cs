using System;
using VideoOS.Common.Integration;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public interface IHardwareConfigDataComponent : IDisposable
{
	int CreateHardwareBlock(Guid hardwareId, DateTime expires, string owner, string data);

	void DeleteHardwareBlock(int id);

	HardwareConfigData QueryHardwareById(Guid hardwareId);

	HardwareConfigData[] QueryHardwareByRecorderId(Guid recorderId);

	void UpdateCredentials(Guid id, string userName, string password, DateTime credentialsLastModified, ICallContextInfo callContextInfo);

	string QueryPasswordById(Guid hardwareId);

	HardwareConfigData[] QueryHardwareWithSameMacAddressById(Guid hardwareId);

	HardwarePasswordConfigData[] QueryHardwareForPasswordChange(bool selectAllHardware, Guid[] activeRecordingServers, Guid[] hardwareIds);
}

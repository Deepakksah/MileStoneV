using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IHardwareComponent : IDisposable
{
	Hardware QueryHardwareById(string hardwareId);

	int CreateHardwareBlock(string hardwareId, DateTime expires, string owner, string data);

	void DeleteHardwareBlock(int id);

	HardwareBlock[] QueryHardwareBlocks();

	Hardware[] QueryHardwareByIds(IEnumerable<string> ids);

	Hardware[] QueryHardwareByRecordingServer(string recordingServerId);

	string[] QueryHardwareIdsByRecorder(string recorderId);

	void UpdateHardware(string id, string name, string description, string hardwareAddress, string userName, bool? enabled, string password, Guid sessionId, DateTime? credentialsLastModified, DateTime? hardwareDataLastUpdated);

	string QueryPasswordById(string id);

	DeviceEvents QueryHardwareEvents(string hardwareId);
}

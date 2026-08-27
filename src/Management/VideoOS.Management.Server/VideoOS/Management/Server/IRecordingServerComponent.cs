using System;

namespace VideoOS.Management.Server;

public interface IRecordingServerComponent : IDisposable
{
	string CreateRecordingServer(string id, string name, string hostName, string webserverHostName, int webserverPort, string timeZoneName, string scanRanges, bool isMulticastLicensed, int failoverUdpPort);

	RecordingServer QueryRecordingServer(string recordingServerId);

	RecordingServer[] QueryRecordingServersByIds(string[] recorderIds);

	string GetIdFromDeviceId(string deviceId);

	Guid? GetIdFromhostedHardwareId(int hostedHardwareId);

	Guid[] GetIdsFromHostingServerId(int hostingServerId);

	string QueryRecordingServerDiskUsage(string recordingServerId);

	string QueryRecordingServerHostName(string recordingServerId);

	string QueryRecordingServerIdFromRecordingServerServiceId(string recordingServerServiceId);

	string[] QueryRecordingServerIds();

	string[] QueryRecordingServerIds(string serverGroupId);

	int QueryEnabledRecordingServersCount();

	void UpdateRecordingServer(string recordingServerId, string name, string description, int? synchronizationTime, CustomProperty[] customProperties, Guid sessionId);

	void UpdateRecordingServerServiceConfigurable(string recordingServerId, bool? publicAccessEnabled, string publicWebAddress, int? publicWebPort, bool? shutdownOnStorageFailure, string multicastServerAddress);

	void UpdateRecordingServerMulticastSettings(string recordingServerId, string multicastSettings);

	void UpdateRecordingServerFailoverSettings(string recordingServerId, string primaryFailoverGroupId, string secondaryFailoverGroupId, int failoverPorts);

	RecordingServerServiceInfo QueryRecordingServerServiceInfo(string recordingServerServiceId);
}

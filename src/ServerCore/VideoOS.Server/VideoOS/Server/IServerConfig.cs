using System;

namespace VideoOS.Server;

public interface IServerConfig
{
	int AutomaticActivationDelaySec { get; }

	int AutomaticActivationRetryDelaySec { get; }

	int MoveHardwareSimultaneousTasks { get; }

	TimeSpan MoveHardwarePrepareMoveHardwareTimeout { get; }

	string OfflineFolder { get; }

	string ConfigFilePath { get; }

	ILogConfig LogConfiguration { get; }

	IRecorderCommunicationConfig RecorderCommunicationConfiguration { get; }

	IVmoCommunicationConfig VmoCommunicationConfiguration { get; }

	IRulesConfig RulesConfiguration { get; }

	IWebApiConfig WebApiConfig { get; }

	ILogDataQueueManagerConfig LogDataQueueManagerConfig { get; }

	IEvidenceLockUpdateConfig EvidenceLockUpdateConfig { get; }

	IEvidenceLockOldRecorderConfig EvidenceLockOldRecorderConfig { get; }

	Guid ClientRegistrationId { get; }

	uint FileStorageQuotaMBLimit { get; }

	bool IsFailoverClusterMember { get; }

	string EncodedClusterPassword { get; }

	string ClusterModuleName { get; }

	string VirtualAddress { get; }

	Uri ClusterLocalWebserverUri { get; }

	int TrayControllerPort { get; }

	void Initialize(int vmoPort);

	void Initialize();
}

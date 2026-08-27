using System;

namespace VideoOS.Management.Server;

public interface IFailoverServerComponent : IDisposable
{
	Guid QueryAssociatedRecorderId(string failoverId);

	FailoverServer[] QueryFailoverServers(string parentId, bool useServerGroupFilter);

	FailoverServer QueryFailoverServer(string id);

	FailoverServer[] QueryFailoverServersByIds(string[] failoverServerIds);

	string CreateFailoverServer(string parentId, FailoverServer failoverServer);

	void UpdateFailoverServer(FailoverServer failoverServer);

	void DeleteFailoverServer(string failoverServerId);

	void MoveToGroup(string id, string targetGroupId, int insertAtPosition);

	void SetEnabled(string id, bool enabled);

	void SetRecorderInformation(string id, bool clientEncryptionEnabled, string webserverHostName, int webserverPort, string version, string timeZone);
}

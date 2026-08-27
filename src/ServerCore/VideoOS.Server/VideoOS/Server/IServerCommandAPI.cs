using System;
using System.Collections;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Server;

public interface IServerCommandAPI
{
	UserInfo GetUserInfo(string token);

	Identity GetUserIdentity(string token);

	string GetVmoServerName();

	int GetVmoServerPort();

	int GetVmoServerSecurePort();

	void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo);

	void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo);

	DictionaryEntry[] GetVmoServicesList();
}

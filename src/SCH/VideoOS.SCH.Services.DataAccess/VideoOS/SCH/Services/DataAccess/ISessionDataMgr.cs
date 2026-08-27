using System;
using System.Collections.Generic;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess;

public interface ISessionDataMgr
{
	bool CreateSession(string clientName, string clientUserName, DateTime expiryTime, out Guid sessionID);

	Session GetSession(Guid sessionID);

	bool DeleteSession(Guid sessionID);

	bool SessionExists(Guid sessionID);

	List<Guid> GetSessionIDsWithExpiryTimeLessThan(DateTime expireTime);

	Dictionary<Guid, Session> GetAllSessions();

	void SetSessionExpireTime(IEnumerable<Guid> sessionIDs, DateTime expiryTime);

	List<ServiceChannelClient> GetAllClients();
}

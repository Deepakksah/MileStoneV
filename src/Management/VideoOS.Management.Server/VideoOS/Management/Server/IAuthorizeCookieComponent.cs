using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public interface IAuthorizeCookieComponent : IDisposable
{
	string CreateAuthorizationCookie(string userSid, AuthorizationActionType action);

	void DeleteCookie(string authorizationId, string userSid, AuthorizationActionType action);

	void DeleteInvalidCookies(string userSid);

	void ExtendCookieValidTime(string authorizationId, int addedTimeSec);

	bool IsAuthorized(string userSid, string authorizationId, AuthorizationActionType action);
}

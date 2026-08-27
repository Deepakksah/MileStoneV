using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public interface IDualAuthorizationHandling
{
	bool CheckDualAuthorization(IGeminiRequestContext context, string authorizationId, AuthorizationActionType action);

	void ExtendCookieTime(IGeminiRequestContext context, string authorizationId, TimeSpan addedTime);

	string Authorize(IGeminiRequestContext context, string userToBeAuthorizedSid, string userToBeAuthorizedDomainName, AuthorizationActionType action);

	void DeleteCookie(IGeminiRequestContext context, string authorizationId, AuthorizationActionType action);
}

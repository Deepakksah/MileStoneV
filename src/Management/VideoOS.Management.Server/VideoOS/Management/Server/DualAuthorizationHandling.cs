using System;
using VideoOS.Management.Server.Log;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public class DualAuthorizationHandling : IDualAuthorizationHandling
{
	private bool _loggingEnabled;

	public DualAuthorizationHandling()
	{
		_loggingEnabled = true;
	}

	public DualAuthorizationHandling(bool enableLogging)
	{
		_loggingEnabled = enableLogging;
	}

	public bool CheckDualAuthorization(IGeminiRequestContext context, string authorizationId, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (!RequiresDualAuthorization(context, context.AuthenticatedUserSid))
		{
			return false;
		}
		AuthorizeCookie.DeleteInvalidCookies(context, context.AuthenticatedUserSid);
		if (AuthorizeCookie.IsAuthorized(context, context.AuthenticatedUserSid, authorizationId, action))
		{
			return true;
		}
		throw new UserNotAuthorizedForLogOnException();
	}

	public void ExtendCookieTime(IGeminiRequestContext context, string authorizationId, TimeSpan addedTime)
	{
		AuthorizeCookie.ExtendCookieValidTime(context, authorizationId, addedTime);
	}

	public string Authorize(IGeminiRequestContext context, string userToBeAuthorizedSid, string userToBeAuthorizedDomainName, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		if (context.AuthenticatedUserSid.Equals(userToBeAuthorizedSid, StringComparison.OrdinalIgnoreCase))
		{
			if (_loggingEnabled)
			{
				using VmoLogManager vmoLogManager = new VmoLogManager(context);
				vmoLogManager.LogEntryAuditLogger.AuditDualAuthorizationSelfAuthorization(context.UserDomainAccountName, context.RemoteIPAddress);
			}
			throw new UserSelfAuthorizeException();
		}
		if (!IsAllowedToAuthorize(context))
		{
			if (_loggingEnabled)
			{
				using VmoLogManager vmoLogManager2 = new VmoLogManager(context);
				vmoLogManager2.LogEntryAuditLogger.AuditDualAuthorizationUserNotAllowedToAuthorize(context.UserDomainAccountName, userToBeAuthorizedDomainName, context.RemoteIPAddress);
			}
			throw new UserNotAllowedToAuthorizeException();
		}
		if (!RequiresDualAuthorization(context, userToBeAuthorizedSid))
		{
			return string.Empty;
		}
		string result = AuthorizeCookie.Authorize(context, userToBeAuthorizedSid, AuthorizationActionType.LogOn);
		if (_loggingEnabled)
		{
			using VmoLogManager vmoLogManager3 = new VmoLogManager(context);
			vmoLogManager3.LogEntryAuditLogger.AuditDualAuthorizationUserAuthorized(context.UserDomainAccountName, userToBeAuthorizedDomainName, context.RemoteIPAddress);
		}
		return result;
	}

	public void DeleteCookie(IGeminiRequestContext context, string authorizationId, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		AuthorizeCookie.Delete(context, authorizationId, context.AuthenticatedUserSid, action);
	}

	private static bool RequiresDualAuthorization(IGeminiRequestContext requestContext, string userSid)
	{
		Role[] array = Role.FindRolesByMember(requestContext, userSid);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].DualAuthorizationRequired)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsAllowedToAuthorize(IGeminiRequestContext requestContext)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		return requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "AUTHORIZE_USER");
	}
}

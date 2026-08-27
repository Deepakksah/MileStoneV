using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public sealed class AuthorizeCookie
{
	public string AuthorizationId { get; set; }

	public string Sid { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ValidUntil { get; set; }

	public AuthorizationActionType ActionType { get; set; }

	public static string Authorize(IGeminiRequestContext context, string userSid, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		using IAuthorizeCookieComponent authorizeCookieComponent = context.CreateAuthorizeCookieComponent();
		return authorizeCookieComponent.CreateAuthorizationCookie(userSid, action);
	}

	public static void Delete(IGeminiRequestContext context, string authorizationId, string userSid, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		using IAuthorizeCookieComponent authorizeCookieComponent = context.CreateAuthorizeCookieComponent();
		authorizeCookieComponent.DeleteCookie(authorizationId, userSid, action);
	}

	public static void DeleteInvalidCookies(IGeminiRequestContext context, string userSid)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		using IAuthorizeCookieComponent authorizeCookieComponent = context.CreateAuthorizeCookieComponent();
		authorizeCookieComponent.DeleteInvalidCookies(userSid);
	}

	public static void ExtendCookieValidTime(IGeminiRequestContext context, string authorizationId, TimeSpan addedTime)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		using IAuthorizeCookieComponent authorizeCookieComponent = context.CreateAuthorizeCookieComponent();
		authorizeCookieComponent.ExtendCookieValidTime(authorizationId, Convert.ToInt32(Math.Floor(addedTime.TotalSeconds)));
	}

	public static bool IsAuthorized(IGeminiRequestContext context, string userSid, string authorizationId, AuthorizationActionType action)
	{
		if (context == null)
		{
			throw new ArgumentNullException("context");
		}
		using IAuthorizeCookieComponent authorizeCookieComponent = context.CreateAuthorizeCookieComponent();
		return authorizeCookieComponent.IsAuthorized(userSid, authorizationId, action);
	}
}

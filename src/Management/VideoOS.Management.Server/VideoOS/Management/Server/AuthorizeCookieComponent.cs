using System;
using System.Globalization;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public sealed class AuthorizeCookieComponent : GeminiResourceComponent, IAuthorizeCookieComponent, IDisposable
{
	public bool IsAuthorized(string userSid, string authorizationId, AuthorizationActionType action)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_authorization_cookie_is_authorized");
			BindSIDType("@UserSid", userSid);
			BindNameType("@AuthorizationId", authorizationId);
			BindNameType("@Action", action.ToString());
			return (bool)ExecuteScalar();
		});
	}

	public string CreateAuthorizationCookie(string userSid, AuthorizationActionType action)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			string text = Guid.NewGuid().ToString();
			DateTime utcNow = DateTime.UtcNow;
			int num = Convert.ToInt32(Config.Instance.GetAppSetting("DualAuthorization_InitialAuthorizationValidTimeSec", "300"), CultureInfo.InvariantCulture);
			DateTime parameterValue = utcNow + TimeSpan.FromSeconds(num);
			PrepareStoredProcedure("create_authorization_cookie");
			BindSIDType("@UserSid", userSid);
			BindNameType("@AuthorizationId", text);
			BindDateTime("@CreatedDate", utcNow);
			BindDateTime("@ValidUntil", parameterValue);
			BindNameType("@Action", action.ToString());
			ExecuteNonQuery();
			return text;
		});
	}

	public void ExtendCookieValidTime(string authorizationId, int addedTimeSec)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("extend_authorization_cookie");
			BindNameType("@AuthorizationId", authorizationId);
			BindInt("@AddedTimeSec", addedTimeSec);
			ExecuteNonQuery();
		});
	}

	public void DeleteInvalidCookies(string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_invalid_authorization_cookie_by_sid");
			BindSIDType("@UserSid", userSid);
			ExecuteNonQuery();
		});
	}

	public void DeleteCookie(string authorizationId, string userSid, AuthorizationActionType action)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_authorization_cookie");
			BindSIDType("@UserSid", userSid);
			BindNameType("@AuthorizationId", authorizationId);
			BindNameType("@Action", action.ToString());
			ExecuteNonQuery();
		});
	}
}

using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Services.Protocols;

namespace VideoOS.Mobile.Utilities;

public static class LoginCheck
{
	public enum LoginCheckResult
	{
		InvalidServerUriFormat,
		InvalidusernameFormat,
		InvalidPasswordFormat,
		LoginFailedServerNotFound,
		LoginFailedInvalidUser,
		LoginSuccess,
		RequireAdministrativeRights
	}

	private const string UnauthorisedAccessExceptionString = "51000";

	private const string UnauthorisedAccessExceptionDueToTimeProfileString = "50000";

	public static LoginCheckResult CheckADLogin(string serverUri, string username, string password)
	{
		Uri uri;
		LoginCheckResult loginCheckResult = ValidateServerUrl(serverUri, out uri);
		NetworkCredential credentials = null;
		if (loginCheckResult == LoginCheckResult.LoginSuccess)
		{
			loginCheckResult = PrepareCredentials(username, password, out credentials);
		}
		if (loginCheckResult == LoginCheckResult.LoginSuccess)
		{
			loginCheckResult = CheckLogin(new ADLoginClient(), uri, credentials);
		}
		if (loginCheckResult == LoginCheckResult.LoginFailedServerNotFound || loginCheckResult == LoginCheckResult.LoginFailedInvalidUser)
		{
			loginCheckResult = CheckLogin(new ADLoginClientLegacy(), uri, credentials);
		}
		return loginCheckResult;
	}

	public static LoginCheckResult CheckBasicLogin(string serverUri, string username, string password)
	{
		Uri uri;
		LoginCheckResult loginCheckResult = ValidateServerUrl(serverUri, out uri);
		NetworkCredential credentials = null;
		if (loginCheckResult == LoginCheckResult.LoginSuccess)
		{
			loginCheckResult = PrepareCredentials(username, password, out credentials);
		}
		if (loginCheckResult == LoginCheckResult.LoginSuccess)
		{
			loginCheckResult = CheckLogin(new BasicUserLoginClient(), uri, credentials);
		}
		if (loginCheckResult == LoginCheckResult.LoginFailedServerNotFound)
		{
			loginCheckResult = CheckLogin(new BasicUserLoginClientLegacy(), uri, credentials);
		}
		return loginCheckResult;
	}

	private static LoginCheckResult ValidateServerUrl(string serverUri, out Uri uri)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		uri = null;
		try
		{
			uri = new Uri(serverUri);
		}
		catch (Exception)
		{
			return LoginCheckResult.InvalidServerUriFormat;
		}
		try
		{
			if (!new HttpClient().GetAsync(uri).Wait(TimeSpan.FromSeconds(5.0)))
			{
				return LoginCheckResult.LoginFailedServerNotFound;
			}
		}
		catch (Exception)
		{
			return LoginCheckResult.LoginFailedServerNotFound;
		}
		return LoginCheckResult.LoginSuccess;
	}

	private static LoginCheckResult PrepareCredentials(string username, string password, out NetworkCredential credentials)
	{
		credentials = null;
		if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
		{
			credentials = CredentialCache.DefaultNetworkCredentials;
		}
		else
		{
			string[] array = username.Split('\\');
			if (array.Length > 2)
			{
				return LoginCheckResult.InvalidusernameFormat;
			}
			credentials = ((array.Length == 1) ? new NetworkCredential(array[0], password) : new NetworkCredential(array[1], password, array[0]));
		}
		return LoginCheckResult.LoginSuccess;
	}

	private static LoginCheckResult CheckLogin(ILoginClient loginClient, Uri serverUrl, NetworkCredential credentials)
	{
		try
		{
			loginClient.Credentials = credentials;
			loginClient.Url = serverUrl;
			loginClient.Login(Guid.NewGuid(), "");
			return LoginCheckResult.LoginSuccess;
		}
		catch (Exception ex)
		{
			WebException ex2 = (ex as WebException) ?? (ex.InnerException as WebException);
			if (ex2 != null)
			{
				if (ex2.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
				{
					return LoginCheckResult.LoginFailedInvalidUser;
				}
			}
			else if (ex is FaultException<ExceptionDetail> ex3)
			{
				if (ex3.Code.Name.Equals("51000", StringComparison.OrdinalIgnoreCase) || ex3.Code.Name.Equals("50000", StringComparison.OrdinalIgnoreCase))
				{
					return LoginCheckResult.LoginFailedInvalidUser;
				}
			}
			else if (ex is SoapException ex4 && (ex4.Detail.InnerText.Equals("51000", StringComparison.OrdinalIgnoreCase) || ex4.Detail.InnerText.Equals("50000", StringComparison.OrdinalIgnoreCase)))
			{
				return LoginCheckResult.LoginFailedInvalidUser;
			}
		}
		return LoginCheckResult.LoginFailedServerNotFound;
	}
}

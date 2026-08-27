using System;
using System.Net;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.LoginThread;

public class MipSdkStandaloneActions
{
	public static void UnregisterGracefully(Uri managementServerUri, Guid serviceId, string user, string pass, bool isAdUser)
	{
		string authenticationType = (isAdUser ? "Negotiate" : "Basic");
		CredentialCache cc = Util.BuildCredentialCache(managementServerUri, user, pass, authenticationType);
		Action action = delegate
		{
			ServiceUtil.UnRegisterServiceUri(EnvironmentManager.Instance.CurrentSite.ServerId, serviceId);
		};
		try
		{
			LoginExecuteLogout(managementServerUri, cc, action);
		}
		catch (ServerNotFoundMIPException)
		{
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static void LoginExecuteLogout(Uri managementServerUri, CredentialCache cc, Action action)
	{
		try
		{
			VideoOS.Platform.SDK.Environment.Initialize();
			VideoOS.Platform.SDK.Environment.AddServer(managementServerUri.Scheme == Uri.UriSchemeHttps, managementServerUri, cc);
			VideoOS.Platform.SDK.Environment.Login(managementServerUri, masterOnly: true);
			action?.Invoke();
		}
		catch
		{
			throw;
		}
		finally
		{
			VideoOS.Platform.SDK.Environment.Logout(managementServerUri);
			VideoOS.Platform.SDK.Environment.RemoveServer(managementServerUri);
			VideoOS.Platform.SDK.Environment.UnInitialize();
		}
	}
}

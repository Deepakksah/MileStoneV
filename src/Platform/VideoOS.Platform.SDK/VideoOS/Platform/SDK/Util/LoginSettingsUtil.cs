using System;
using System.Net;
using VideoOS.Platform.Common.OAuth;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Proxy;

namespace VideoOS.Platform.SDK.Util;

public class LoginSettingsUtil
{
	public static LoginSettings LoginOAuth(Uri uri, CredentialCache credentialCache, bool secureOnly)
	{
		MipTokenCacheInternal mipTokenCacheInternal = null;
		try
		{
			Uri address = new Uri(uri, "idp");
			bool isBasic;
			NetworkCredential networkCredential = CredentialUtil.ConvertCredentialCacheToNetworkCredential(uri, credentialCache, out isBasic);
			mipTokenCacheInternal = new MipTokenCacheInternal(address, networkCredential, isBasic);
			LoginSettings loginSettings = new LoginSettings(uri, credentialCache, mipTokenCacheInternal, Guid.Empty, uri.Host, "XPCO", secureOnly);
			loginSettings.Guid = GetServerId(loginSettings);
			return loginSettings;
		}
		catch
		{
			mipTokenCacheInternal?.Dispose();
			return null;
		}
	}

	public static Guid GetServerId(LoginSettings loginSettings)
	{
		if (loginSettings.IdentityTokenCache == null)
		{
			throw new ArgumentException("LoginSettings.IdentityTokenCache must not be null", "loginSettings");
		}
		try
		{
			using ServerClient serverClient = new ServerClient(loginSettings, ManagementServerWcfHelper.GetOAuthBinding(loginSettings.Uri.Scheme == Uri.UriSchemeHttps));
			serverClient.Initialize();
			serverClient.CheckAuthentication();
			return new Guid(serverClient.GetServerInformation().InstanceId);
		}
		catch (Exception innerException)
		{
			throw new InvalidOperationException("Failed to get server ID from the server.", innerException);
		}
	}
}

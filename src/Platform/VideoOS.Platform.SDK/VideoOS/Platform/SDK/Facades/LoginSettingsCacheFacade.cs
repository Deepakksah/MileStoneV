using System;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Facades;

public class LoginSettingsCacheFacade : ILoginSettingsCacheFacade
{
	public LoginSettings GetLoginSettings(Guid serverGuid)
	{
		return LoginSettingsCache.GetLoginSettings(serverGuid);
	}

	public bool Contains(LoginSettings ls)
	{
		return LoginSettingsCache.LoginSettings.Contains(ls);
	}

	public bool Remove(LoginSettings ls)
	{
		return LoginSettingsCache.LoginSettings.Remove(ls);
	}

	public void Clear()
	{
		LoginSettingsCache.Clear();
	}
}

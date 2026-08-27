namespace VideoOS.Platform.Login;

public class LoginSettingsCacheWrapper : ILoginSettingsCacheWrapper
{
	public LoginSettings GetLoginSettings(ServerId serverId)
	{
		return LoginSettingsCache.GetLoginSettings(serverId);
	}
}

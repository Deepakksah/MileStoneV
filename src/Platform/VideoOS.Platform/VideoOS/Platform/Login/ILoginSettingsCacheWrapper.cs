namespace VideoOS.Platform.Login;

public interface ILoginSettingsCacheWrapper
{
	LoginSettings GetLoginSettings(ServerId serverId);
}

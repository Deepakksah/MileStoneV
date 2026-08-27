namespace VideoOS.Management.Server;

public interface IConfig
{
	string GetAppSetting(string name, string defaultValue);
}

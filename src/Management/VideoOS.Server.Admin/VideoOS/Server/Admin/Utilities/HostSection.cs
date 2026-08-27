using Microsoft.Web.Administration;

namespace VideoOS.Server.Admin.Utilities;

public class HostSection
{
	private readonly Configuration _appHostConfiguration;

	private readonly string _virtualLocation;

	public ConfigurationSection AnonymousAuthentication => _appHostConfiguration.GetSection("system.webServer/security/authentication/anonymousAuthentication", _virtualLocation);

	public ConfigurationSection BasicAuthentication => _appHostConfiguration.GetSection("system.webServer/security/authentication/basicAuthentication", _virtualLocation);

	public ConfigurationSection WindowsAuthentication => _appHostConfiguration.GetSection("system.webServer/security/authentication/windowsAuthentication", _virtualLocation);

	public ConfigurationSection DigestAuthentication => _appHostConfiguration.GetSection("system.webServer/security/authentication/digestAuthentication", _virtualLocation);

	public ConfigurationSection Access => _appHostConfiguration.GetSection("system.webServer/security/access", _virtualLocation);

	public HostSection(ServerManager serverManager, string virtualLocation)
	{
		_appHostConfiguration = serverManager.GetApplicationHostConfiguration();
		_virtualLocation = virtualLocation;
	}
}

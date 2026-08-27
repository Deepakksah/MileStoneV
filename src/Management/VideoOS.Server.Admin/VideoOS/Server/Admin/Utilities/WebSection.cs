using Microsoft.Web.Administration;

namespace VideoOS.Server.Admin.Utilities;

public class WebSection
{
	private readonly Configuration _webConfiguration;

	private readonly string _virtualLocation;

	public ConfigurationSection Handlers => _webConfiguration.GetSection("system.webServer/handlers", _virtualLocation);

	public ConfigurationSection DirectoryBrowse => _webConfiguration.GetSection("system.webServer/directoryBrowse", _virtualLocation);

	public ConfigurationSection HttpRedirect => _webConfiguration.GetSection("system.webServer/httpRedirect", _virtualLocation);

	public ConfigurationSection DefaultDocument => _webConfiguration.GetSection("system.webServer/defaultDocument", _virtualLocation);

	public WebSection(ServerManager serverManager, string virtualLocation)
	{
		_webConfiguration = serverManager.GetApplicationHostConfiguration();
		_virtualLocation = virtualLocation;
	}
}

using System;
using System.Collections.Generic;
using VideoOS.Mobile.Management;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Mobile.ManagementServer;

public class ManagementService
{
	private ManagementServerOptions _serverOptions;

	private readonly object _serverOptionsLock = new object();

	public static ManagementService Instance = new ManagementService();

	public ManagementServerOptions ServerOptions
	{
		get
		{
			return _serverOptions;
		}
		private set
		{
			lock (_serverOptionsLock)
			{
				_serverOptions = value;
			}
		}
	}

	public void Refresh()
	{
		Log.TraceBegin("Refresh", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\ManagementServer\\ManagementService.cs", 39);
		try
		{
			ToolOptionServerLog serverLogOptions = ToolOptions.GetServerLogOptions();
			ToolOptionPrivacySetting privacySettingOptions = ToolOptions.GetPrivacySettingOptions();
			ToolOptionBookmark bookmarkOptions = ToolOptions.GetBookmarkOptions();
			ICollection<LoginProvider> loginProviders = LoginProviders.GetLoginProviders();
			ServerOptions = new ManagementServerOptions(serverLogOptions, privacySettingOptions, bookmarkOptions, loginProviders);
		}
		catch (Exception exception)
		{
			Log.Error("ManagementService.Refresh()", "Exception getting Tools Options. Default values will be used");
			Log.Exception(exception);
			ServerOptions = new ManagementServerOptions();
		}
		Log.TraceEnd("Refresh", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Mobile\\MobileServer\\MobileServerBinaries\\VideoOS.Mobile\\src\\ManagementServer\\ManagementService.cs", 57);
	}
}

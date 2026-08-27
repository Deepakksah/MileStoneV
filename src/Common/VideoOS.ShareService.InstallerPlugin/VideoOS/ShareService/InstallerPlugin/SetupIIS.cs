using System;
using System.DirectoryServices;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.ShareService.InstallerPlugin;

public class SetupIIS
{
	private IisHelper _iisHelper;

	private LogManagerWrapper _logComponent;

	public const string ShareServiceAppPoolName = "VideoOS ShareService AppPool";

	public const string ShareServiceVirtualDirName = "Share";

	public SetupIIS(LogManagerWrapper logComponent)
	{
		_logComponent = logComponent;
		_iisHelper = new IisHelper((ILogger)(object)_logComponent);
	}

	public void Install(string virtualDirectoryPhysicalPath, string websiteId, string websiteAddress, AccountInfo serviceAccount)
	{
		_logComponent.Write("Start of SetupIIS Install");
		if (serviceAccount == null)
		{
			throw new ArgumentException("serviceAccount");
		}
		if (string.IsNullOrEmpty(virtualDirectoryPhysicalPath))
		{
			throw new ArgumentException("virtualDirectoryPhysicalPath");
		}
		if (string.IsNullOrEmpty(websiteId))
		{
			throw new ArgumentException("websiteId");
		}
		if (string.IsNullOrEmpty(websiteAddress))
		{
			throw new ArgumentException("websiteAddress");
		}
		if (!Directory.Exists(virtualDirectoryPhysicalPath))
		{
			throw new DirectoryNotFoundException(virtualDirectoryPhysicalPath);
		}
		SetupAppPools(serviceAccount);
		CreateVirtualDirectory(websiteId, "Share", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, virtualDirectoryPhysicalPath, "VideoOS ShareService AppPool");
		_iisHelper.Control(IisAction.Start);
	}

	public void Uninstall(string websiteId)
	{
		VideoOS.Server.Admin.Utilities.VirtualDirectory.DeleteVirtualDirectory(websiteId, "Share");
		using AppPoolHelper appPoolHelper = new AppPoolHelper((ILogger)(object)_logComponent);
		DeleteAppPool(appPoolHelper, "VideoOS ShareService AppPool");
	}

	private void SetupAppPools(AccountInfo serviceAccount)
	{
		using (AppPoolHelper appPoolHelper = new AppPoolHelper((ILogger)(object)_logComponent))
		{
			appPoolHelper.CreateApplicationPool("VideoOS ShareService AppPool", serviceAccount, classicManagedPipelineMode: false, AppPoolHelper.StartMode.AlwaysRunning, "");
		}
		_logComponent.Write("Share Service AppPool is created.");
	}

	private void CreateVirtualDirectory(string websiteId, string virtualApplicationPath, AuthorizationTypes authorization, string physicalPath, string appPoolName)
	{
		VideoOS.Server.Admin.Utilities.VirtualDirectory virtualDirectory = new VideoOS.Server.Admin.Utilities.VirtualDirectory(websiteId, appPoolName, virtualApplicationPath, physicalPath);
		virtualDirectory.Authorization = authorization;
		virtualDirectory.SslSettings = VideoOS.Server.Admin.Utilities.SslFlags.Ssl;
		virtualDirectory.CommitChanges();
		_logComponent.Write("Virtual directory is created: " + virtualDirectory.VirtualLocation);
	}

	private static void DeleteAppPool(AppPoolHelper appPoolHelper, string appPoolName)
	{
		try
		{
			ApplicationPool appPool = appPoolHelper.GetAppPool(appPoolName);
			if (appPool != null)
			{
				appPoolHelper.DeleteApplicationPool(appPool);
			}
		}
		catch (DirectoryNotFoundException)
		{
		}
		catch (DirectoryServicesCOMException)
		{
		}
	}
}

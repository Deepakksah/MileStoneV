using System;
using System.DirectoryServices;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;
using VideoOS.GatewayService.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.InstallerPlugin;

public class SetupIIS : ISetupIIS
{
	private IIisHelperWrapper _iisHelper;

	private ILogger _logComponent;

	private IFileSystemHelper _fileSystemHelper;

	private IAppPoolHelperWrapperFactory _appPoolHelperFactory;

	private IVirtualDirectoryHelper _virtualDirectoryHelper;

	public const string ApiGatewayAppPoolName = "VideoOS ApiGateway AppPool";

	private const string DefaultWebSiteId = "1";

	public SetupIIS()
	{
		_logComponent = LogManager.InnerLog;
		_iisHelper = new IisHelperWrapper(_logComponent);
		_fileSystemHelper = new FileSystemHelper();
		_appPoolHelperFactory = new AppPoolHelperWrapperFactory();
		_virtualDirectoryHelper = new VirtualDirectoryHelper();
	}

	public SetupIIS(ILogger logComponent, IIisHelperWrapper iisHelper, IFileSystemHelper fileSystemHelper, IAppPoolHelperWrapperFactory appPoolHelperFactory, IVirtualDirectoryHelper virtualDirectoryHelper)
	{
		_logComponent = logComponent;
		_iisHelper = iisHelper;
		_fileSystemHelper = fileSystemHelper;
		_appPoolHelperFactory = appPoolHelperFactory;
		_virtualDirectoryHelper = virtualDirectoryHelper;
	}

	public void SetupSecureBinding(string websiteId, string managementUri)
	{
		if (string.Equals(websiteId, "1") && new UriBuilder(managementUri).Uri.Scheme == Uri.UriSchemeHttps && !_iisHelper.CheckSecureBindings(websiteId))
		{
			_iisHelper.CreateCertificateAndHttpsBindingIfNeeded(websiteId);
		}
	}

	public void Install(string virtualDirectoryPhysicalPath, string websiteId, string websiteAddress, AccountInfo serviceAccount, Action registerGatewayServiceAction)
	{
		if (serviceAccount == null)
		{
			throw new ArgumentException("Service account is null", "serviceAccount");
		}
		if (string.IsNullOrEmpty(virtualDirectoryPhysicalPath))
		{
			throw new ArgumentException("Physical directory folder is null or empty", "virtualDirectoryPhysicalPath");
		}
		if (string.IsNullOrEmpty(websiteId))
		{
			throw new ArgumentException("Website id is null or empty", "websiteId");
		}
		if (string.IsNullOrEmpty(websiteAddress))
		{
			throw new ArgumentException("Website address is null or empty", "websiteAddress");
		}
		if (!_fileSystemHelper.DirectoryExists(virtualDirectoryPhysicalPath))
		{
			throw new ArgumentException("The directory specified for the virtual directories was not found, virtualDirectoryParentFolder");
		}
		_iisHelper.GrantAccessToIis(serviceAccount.AccountName);
		LoggerExtensions.LogDebug(_logComponent, string.Format(CultureInfo.InvariantCulture, "Granted access to the IIS User: {0}", serviceAccount.AccountName), Array.Empty<object>());
		SetupAppPools(serviceAccount);
		CreateVirtualDirectory(websiteId, "API", AuthorizationTypes.Ntlm | AuthorizationTypes.Anonymous, virtualDirectoryPhysicalPath, "VideoOS ApiGateway AppPool");
		_iisHelper.Control(IisAction.Start);
		registerGatewayServiceAction?.Invoke();
	}

	public void Uninstall(string websiteId)
	{
		_virtualDirectoryHelper.DeleteVirtualDirectory(websiteId, "API");
		using IAppPoolHelperWrapper appPoolHelper = _appPoolHelperFactory.CreateAppPoolHelperWrapper(_logComponent);
		DeleteAppPool(appPoolHelper, "VideoOS ApiGateway AppPool");
	}

	private void SetupAppPools(AccountInfo serviceAccount)
	{
		using (IAppPoolHelperWrapper appPoolHelperWrapper = _appPoolHelperFactory.CreateAppPoolHelperWrapper(_logComponent))
		{
			appPoolHelperWrapper.CreateApplicationPool("VideoOS ApiGateway AppPool", serviceAccount, classicManagedPipelineMode: false, AppPoolHelper.StartMode.AlwaysRunning, "");
		}
		LoggerExtensions.LogDebug(_logComponent, "The AppPool is created.", Array.Empty<object>());
	}

	private void CreateVirtualDirectory(string websiteId, string virtualApplicationPath, AuthorizationTypes authorization, string physicalPath, string appPoolName)
	{
		IVirtualDirectoryWrapper virtualDirectoryWrapper = _virtualDirectoryHelper.CreateVirtualDirectory(websiteId, appPoolName, virtualApplicationPath, physicalPath);
		virtualDirectoryWrapper.Authorization = authorization;
		virtualDirectoryWrapper.CommitChanges();
		LoggerExtensions.LogDebug(_logComponent, string.Format(CultureInfo.InvariantCulture, "The virtual directory is created: {0}", virtualDirectoryWrapper.VirtualLocation), Array.Empty<object>());
	}

	private static void DeleteAppPool(IAppPoolHelperWrapper appPoolHelper, string appPoolName)
	{
		try
		{
			AppPoolWrapper appPool = appPoolHelper.GetAppPool(appPoolName);
			if (appPool.AppPool != null)
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

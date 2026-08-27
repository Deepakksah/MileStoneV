using System;
using Microsoft.Extensions.Logging;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class AppPoolHelperWrapper : AppPoolHelper, IAppPoolHelperWrapper, IDisposable
{
	public AppPoolHelperWrapper(ILogger logComponent)
		: base(logComponent)
	{
	}

	public void DeleteApplicationPool(AppPoolWrapper appPoolToDelete)
	{
		DeleteApplicationPool(appPoolToDelete.AppPool);
	}

	AppPoolWrapper IAppPoolHelperWrapper.GetAppPool(string appPoolName)
	{
		return new AppPoolWrapper(GetAppPool(appPoolName));
	}

	void IAppPoolHelperWrapper.CreateApplicationPool(string newApplicationPoolName, AccountInfo account, bool classicManagedPipelineMode, StartMode startMode, string managedRuntimeVersion)
	{
		CreateApplicationPool(newApplicationPoolName, account, classicManagedPipelineMode, startMode, managedRuntimeVersion);
	}
}

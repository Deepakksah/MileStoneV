using System;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public interface IAppPoolHelperWrapper : IDisposable
{
	void CreateApplicationPool(string newApplicationPoolName, AccountInfo account, bool classicManagedPipelineMode, AppPoolHelper.StartMode startMode, string managedRuntimeVersion);

	AppPoolWrapper GetAppPool(string appPoolName);

	void DeleteApplicationPool(AppPoolWrapper appPoolToDelete);
}

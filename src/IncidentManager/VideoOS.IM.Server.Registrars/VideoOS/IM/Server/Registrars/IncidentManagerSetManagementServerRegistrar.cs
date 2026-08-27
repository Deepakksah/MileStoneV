using System;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IM.Server.Service.OEM;
using VideoOS.IM.Server.Service.WindowsRegistries;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars;

public class IncidentManagerSetManagementServerRegistrar : SetManagementServerRegistrarBase
{
	private readonly IWinRegistryService _winRegistryService;

	private const string RegistryKeyName = "ManagementServerAddress";

	public IncidentManagerSetManagementServerRegistrar()
		: this(new TimeSpan(0, 3, 0), new WinRegistryService(), new ServiceControllerFactory(), new WebRequestFactory())
	{
	}

	public IncidentManagerSetManagementServerRegistrar(TimeSpan timeout, IWinRegistryService winRegistryService, IServiceControllerFactory serviceControllerFactory, IWebRequestFactory webRequestFactory)
		: base(null, timeout, serviceControllerFactory, webRequestFactory)
	{
		winRegistryService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IncidentManagerSetManagementServerRegistrar.cs", 29);
		_winRegistryService = winRegistryService;
	}

	protected override Status Execute(Uri newManagementServerAddress)
	{
		string text;
		try
		{
			text = _winRegistryService.WriteRegistryValue("ManagementServerAddress", newManagementServerAddress.ToString());
		}
		catch (Exception ex)
		{
			return LoggedErrorStatus(ex, StatusCode.UnhandledException, string.Format("An error occurred while writing to the registry key-value pair {0},Exception = {1}", "ManagementServerAddress", ex), "WriteRegistryValue");
		}
		if (text != null)
		{
			return LoggedStatus(Status.SuccessStatus(), "Registry key-value pair ManagementServerAddress  updated successfully", "Execute");
		}
		return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "An unexpected error occurred while writing to registry", "Execute");
	}

	protected override Status ExecuteCleanup()
	{
		try
		{
			_winRegistryService.DeleteRegSubKey();
		}
		catch (Exception ex)
		{
			return LoggedErrorStatus(ex, StatusCode.UnhandledException, $"Could not delete registry sub key for {OemResourceService.Instance.IncidentManagerApplicationName}, Exception = {ex}", "DeleteRegSubKey");
		}
		return LoggedStatus(Status.SuccessStatus(), "Registry sub-key ManagementServerAddress has been deleted successfully.", "ExecuteCleanup");
	}
}

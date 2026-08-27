using System;
using System.IO;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.GatewayService.Utilities;
using VideoOS.GatewayService.Utilities.Oem;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.GatewayService.Registrars;

public class GatewayServiceSetManagementServerRegistrar : SetManagementServerRegistrarBase
{
	public IRegistryHelper RegistryHelper{ get; set; }

	public IConfigFileHelper ConfigFileHelper{ get; set; }

	public GatewayServiceSetManagementServerRegistrar()
		: this(new RegistryHelper(new GatewayServiceOemResource()), new TimeSpan(0, 3, 0), new ServiceControllerFactory(), new WebRequestFactory())
	{
	}

	public GatewayServiceSetManagementServerRegistrar(IRegistryHelper registryHelper, TimeSpan timeout, IServiceControllerFactory serviceControllerFactory, IWebRequestFactory webRequestFactory)
		: base(null, timeout, serviceControllerFactory, webRequestFactory)
	{
		RegistryHelper = registryHelper;
	}

	protected override Status ExecuteInitialize()
	{
		ConfigFileHelper = new ConfigFileHelper(base.Logger);
		return base.ExecuteInitialize();
	}

	protected override Status Execute(Uri newManagementServerAddress)
	{
		base.Logger.LogInfo("GatewayServiceSetManagementServerRegistrar.Execute, parameter: " + newManagementServerAddress.ToString());
		string gatewayConfigPath = Path.Combine(RegistryHelper.ReadKey(RegistryKeys.DestinationFolder), "appsettings.json");
		if (ConfigFileHelper.UpdateManagementServerUri(gatewayConfigPath, newManagementServerAddress))
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.Success), "ManagementServer address successfully changed.", "Execute");
		}
		return Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration);
	}
}

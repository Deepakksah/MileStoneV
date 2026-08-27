using System;
using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Server.UACParts;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Crypto;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.Integration;
using VideoOS.Server.Admin.Utilities;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class ManagementServerClientEndpointRegistrar : ClientEndpointRegistrarBase
{
	public ManagementServerClientEndpointRegistrar()
		: this(new FileService(), OemResources.Instance.Server)
	{
	}

	public ManagementServerClientEndpointRegistrar(IFileService fileService, IServerResources oemServerResources)
		: base(new ParametersHandler(fileService, oemServerResources, "Server Service"), new FileSecretProvider(oemServerResources?.CommonApplicationDataFolder, "idp.tmp", "idp.sec"), oemServerResources?.ServiceName)
	{
	}

	protected override Status Execute(NetworkCredential identityServerCredentials, Uri authorizationService)
	{
		Status result = base.Execute(identityServerCredentials, authorizationService);
		IisHelper iisHelper = new IisHelper(NullLogger.Instance);
		RegistrationIisManagementService registrationIisManagementService = new RegistrationIisManagementService();
		string iisConfigFilePath = ServerTrayUtility.ResolveManagementServerWebConfig(iisHelper);
		registrationIisManagementService.RegisterIisManagementService(iisConfigFilePath, authorizationService, base.Logger);
		string reportConfigFilePath = ServerTrayUtility.ResolveReportServerWebConfig(iisHelper);
		registrationIisManagementService.RegisterReportService(reportConfigFilePath, authorizationService, base.Logger);
		return result;
	}

	protected override void AdditionalCleanup()
	{
		RegistrationIisManagementService registrationIisManagementService = new RegistrationIisManagementService();
		try
		{
			registrationIisManagementService.CleanupReportService();
		}
		catch (RemoveFileSecretException ex)
		{
			base.Logger?.LogError(ex.ToString());
		}
	}
}

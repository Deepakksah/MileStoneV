using System;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerConfiguratorData
{
	public static readonly Guid ManagementServerId = new Guid("2CA9F093-7A8E-42A6-968F-750F15B6CB36");

	public static readonly Guid ManagementServerClientId = new Guid("F48D6874-DBC0-4DF9-A012-3415FF13AE5C");

	public static readonly Guid ManagementServerConfigurationElementId = new Guid("86FC038E-26D2-4058-AA9F-417161D3F178");

	public const string ManagementServerRegisterImplementationType = "VideoOS.Server.CertificateRegistration.ManagementServerEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string ManagementClientRegisterImplementationType = "VideoOS.Server.CertificateRegistration.ManagementServerClientEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string ManagementServerSystemConfigurationImplementationType = "VideoOS.Server.CertificateRegistration.ManagementServerSystemConfigurationRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid ReportServerId = new Guid("FF358961-2F09-4AC1-8FF1-9A8488FE2C4D");

	public const string ReportServerRegisterThisServerImplementationType = "VideoOS.Server.CertificateRegistration.ReportServerRegisterService, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string ReportServerRegisterImplementationType = "VideoOS.Server.CertificateRegistration.ReportServerEndpointRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid DownloadManagerClientId = new Guid("4DD7B894-0238-4E80-A237-B2D4C425AE4E");

	public static readonly string DownloadManagerSetManagementServerImplementationType = "VideoOS.Server.CertificateRegistration.DownloadManagerSetManagementServerRegistrar, VideoOS.Server.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid IncidentManagerClientId = new Guid("DDBE73F1-7804-4630-8F4D-DD2A43DA3965");

	public const string IncidentManagerServerAddressRegistrarImplementationType = "VideoOS.IM.Server.Registrars.IncidentManagerSetManagementServerRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string IncidentManagerClientEndpointRegistrarImplementationType = "VideoOS.IM.Server.Registrars.IncidentManagerClientEndpointRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid IncidentManagerServerId = new Guid("20DFAF9C-FD74-4EDD-BD19-A6C0983E4EC2");

	public const string IncidentManagerRegisterEndpointRegistrarImplementationType = "VideoOS.IM.Server.Registrars.IncidentManagerRegisterEndpointRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string IncidentManagerServiceRegistrationUpdateRegistrarImplementationType = "VideoOS.IM.Server.Registrars.IncidentManagerServiceRegistrationUpdateRegistrar, VideoOS.IM.Server.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

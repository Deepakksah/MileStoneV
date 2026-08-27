using System;

namespace VideoOS.Mobile.InstallerWizard;

public static class ServerConfiguratorData
{
	public static readonly Guid MobileServerCertificateGroupId = new Guid("76CFC719-A852-4210-913E-703EADAB139A");

	public static readonly Guid MobileServerEndpointId = new Guid("7337C2DE-9429-499F-9CFE-F3C4E119D0D5");

	public const string MobileServerCertificateRegistrationImplementationType = "VideoOS.Mobile.CertificateRegistration.MobileServerEndpointRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string MobileComponentPresentationType = "";

	public static readonly Guid MobileServerClientEndpointId = new Guid("EC482CE9-2814-4316-A492-8EBC0BC029F7");

	public const string MobileServerSetManagementServerImplementationType = "VideoOS.Mobile.CertificateRegistration.MobileServerSetManagementServerRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string MobileServerRegisterClientRegistrarImplementationType = "VideoOS.Mobile.CertificateRegistration.MobileServerRegisterClientRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string MobileServerServiceRegistrarImplementationType = "VideoOS.Mobile.CertificateRegistration.MobileServerServiceRegistrar, VideoOS.Mobile.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

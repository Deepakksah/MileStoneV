using System;

namespace VideoOS.Installer.RecorderPlugins;

public static class ServerConfiguratorData
{
	public static readonly Guid RecorderServerId = new Guid("B506951E-9841-4846-B02D-61E8AFC12169");

	public static readonly Guid RecorderClientId = new Guid("287BC40C-4375-4719-8450-F7B0352CCB64");

	public static readonly Guid FailoverServerId = new Guid("01AB8C08-4388-40B9-AF41-4992E2F5A396");

	public static readonly Guid FailoverClientId = new Guid("CF8C1572-BE1B-41A6-A7BE-B4911EA41957");

	public const string RecorderServerRegisterImplementationType = "VideoOS.Recorder.CertificateRegistration.RecorderServerEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string RecorderClientRegisterImplementationType = "VideoOS.Recorder.CertificateRegistration.RecorderClientEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string RecorderSetServerAddressImplementationType = "VideoOS.Recorder.CertificateRegistration.RecorderSetManagementServerRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string FailoverServerRegisterImplementationType = "VideoOS.Failover.CertificateRegistration.FailoverServerEndpointRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string FailoverClientRegisterImplementationType = "VideoOS.Failover.CertificateRegistration.FailoverClientEndpointRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string FailoverSetServerAddressImplementationType = "VideoOS.Failover.CertificateRegistration.FailoverSetManagementServerRegistrar, VideoOS.Failover.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string RecorderComponentPresentationType = "";

	public static readonly Guid ImageServerCertificateGroupId = new Guid("549DF21D-047C-456B-958E-99E65DD8B3EC");

	public static readonly Guid ImageServerServerId = new Guid("503D9027-FF08-4F08-998B-CB798898EDBA");

	public const string ImageServerRegisterImplementationType = "VideoOS.Recorder.CertificateRegistration.ImageServerEndpointRegistrar, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string ImageServerComponentPresentationType = "";

	public const string RecorderRegisterThisServerImplementationType = "VideoOS.Recorder.CertificateRegistration.RecorderServerRegisterService, VideoOS.Recorder.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

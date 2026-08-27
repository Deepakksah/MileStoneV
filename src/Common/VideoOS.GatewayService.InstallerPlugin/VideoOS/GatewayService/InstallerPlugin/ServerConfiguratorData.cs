using System;

namespace VideoOS.GatewayService.InstallerPlugin;

public static class ServerConfiguratorData
{
	public static readonly Guid GatewayServiceId = new Guid("BBDE5719-C18D-4224-BF3C-5BF9CFDE4D04");

	public const string GatewayServiceServerRegisterImplementationType = "VideoOS.GatewayService.Registrars.GatewayServiceEndpointRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string GatewayServiceRegisterRegistrationUpdateImplementationType = "VideoOS.GatewayService.Registrars.GatewayServiceRegistrationUpdateRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid GatewayServiceClientId = new Guid("D7F30FCE-9228-4DD7-82A1-3A5036741C6C");

	public const string GatewayServiceClientRegisterImplementationType = "VideoOS.GatewayService.Registrars.GatewayServiceClientEndpointRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string GatewayServiceSetManagementServerRegistrarImplementationType = "VideoOS.GatewayService.Registrars.GatewayServiceSetManagementServerRegistrar, VideoOS.GatewayService.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

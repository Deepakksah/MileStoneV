using System;

namespace VideoOS.Installer.LogServerPlugins;

public static class ServerConfiguratorData
{
	public static readonly Guid LogServerId = new Guid("822B1194-845B-4372-AE1A-451ACD682A55");

	public static readonly Guid LegacyLogServerId = new Guid("7074E620-38CD-47E2-A88D-FB986B52DB49");

	public const string LogServerRegisterThisServiceImplementationType = "VideoOS.LogServer.Registrars.LogServerRegisterServiceRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string LegacyLogServerRegisterThisServiceImplementationType = "VideoOS.LogServer.Registrars.LegacyLogServerRegisterServiceRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string LogServerCertificateRegistrarImplementationType = "VideoOS.LogServer.Registrars.LogServerCertificateRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public static readonly Guid LogServerClientId = new Guid("943FAEF0-0378-4A85-A3DF-AF7D4AC987F3");

	public const string LogServerSetManagementServerImplementationType = "VideoOS.LogServer.Registrars.LogServerSetManagementServerRegistrar, VideoOS.LogServer.Registrars, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

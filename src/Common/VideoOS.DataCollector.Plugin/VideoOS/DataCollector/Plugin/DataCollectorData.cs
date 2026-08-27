using System;

namespace VideoOS.DataCollector.Plugin;

public static class DataCollectorData
{
	public static readonly Guid DataCollectorServerId = new Guid("7907CCD2-9A0C-4756-BF39-476DE2888796");

	public static readonly Guid DataCollectorClientId = new Guid("B40D1543-C8C3-4CAC-9C16-7877C577ED5A");

	public const string DataCollectorServerRegisterImplementationType = "VideoOS.DataCollector.CertificateRegistration.DataCollectorServerEndpointRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string DataCollectorClientRegisterImplementationType = "VideoOS.DataCollector.CertificateRegistration.DataCollectorClientEndpointRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";

	public const string DataCollectorServerSetManagementServerImplementationType = "VideoOS.DataCollector.CertificateRegistration.DataCollectorSetManagementServerRegistrar, VideoOS.DataCollector.CertificateRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bc60fba4a7969f89";
}

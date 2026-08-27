using System;
using System.Net;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class SslConfigurationWrapper : ISslConfiguration, ISslConfigurationKey
{
	private SslConfiguration Config { get; }

	public IPAddress Address => Config.Address;

	public ushort Port => Config.Port;

	public Guid ApplicationId => Config.AppId;

	public string CertificateStoreName => Config.CertStoreName;

	public ClientCertificateCheckMode DefaultCertCheckMode => Config.CertCheckMode.ToManaged();

	public uint DefaultRevocationFreshnessTime => Config.RevocationFreshnessTime;

	public uint DefaultRevocationUrlRetrievalTimeout => Config.RevocationUrlRetrievalTimeout;

	public string DefaultSslCtlIdentifier => Config.SslCtlIdentifier;

	public string DefaultSslCtlStoreName => Config.SslCtlStoreName;

	public SslConfigOptions DefaultFlags => Config.Flags.ToManaged();

	public SslConfigurationWrapper(SslConfiguration config)
	{
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
		Config = config;
	}

	public byte[] GetCertificateHash()
	{
		return Config.CertificateHash;
	}
}

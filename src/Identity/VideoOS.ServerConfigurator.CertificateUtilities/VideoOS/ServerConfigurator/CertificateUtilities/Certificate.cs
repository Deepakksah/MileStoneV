using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateUtilities;

[ExcludeFromCodeCoverage]
public class Certificate : ICertificate
{
	private static class WellKnownOidValues
	{
		public const string BasicConstraints = "2.5.29.19";

		public const string EnhancedKeyUsage = "2.5.29.37";

		public const string ServerAuthentication = "1.3.6.1.5.5.7.3.1";

		public const string ClientAuthentication = "1.3.6.1.5.5.7.3.2";
	}

	private readonly X509Certificate2 _source;

	private bool? _certificateAuthority;

	private bool? _serverAuthentication;

	private bool? _clientAuthenitcation;

	public string FriendlyName => _source.FriendlyName;

	public string IssuerName => _source.IssuerName.Name;

	public string Subject => _source.Subject;

	public bool HasPrivateKey => _source.HasPrivateKey;

	public string SubjectName => _source.SubjectName.Name;

	public DateTime ValidFrom => _source.NotBefore;

	public DateTime ValidUntil => _source.NotAfter;

	public string Thumbprint => _source.Thumbprint;

	public bool CertificateAuthority
	{
		get
		{
			bool? flag = (_certificateAuthority = _certificateAuthority ?? GetCertificateAuthority());
			return flag.Value;
		}
	}

	public bool ServerAuthentication
	{
		get
		{
			bool? flag = (_serverAuthentication = _serverAuthentication ?? GetEnhancedKeyUsage("1.3.6.1.5.5.7.3.1"));
			return flag.Value;
		}
	}

	public bool ClientAuthentication
	{
		get
		{
			bool? flag = (_clientAuthenitcation = _clientAuthenitcation ?? GetEnhancedKeyUsage("1.3.6.1.5.5.7.3.2"));
			return flag.Value;
		}
	}

	public X509Certificate2 X509Certificate => _source;

	public Certificate(X509Certificate2 source)
	{
		_source = source;
	}

	private bool GetEnhancedKeyUsage(string keyUsage)
	{
		X509Extension x509Extension = FindExtension(_source.Extensions, "2.5.29.37");
		if (x509Extension != null)
		{
			return ((X509EnhancedKeyUsageExtension)x509Extension).EnhancedKeyUsages[keyUsage] != null;
		}
		return false;
	}

	private bool GetCertificateAuthority()
	{
		X509Extension x509Extension = FindExtension(_source.Extensions, "2.5.29.19");
		if (x509Extension != null)
		{
			return ((X509BasicConstraintsExtension)x509Extension).CertificateAuthority;
		}
		return false;
	}

	private static X509Extension FindExtension(X509ExtensionCollection extensions, string oidValue)
	{
		return extensions.Cast<X509Extension>().FirstOrDefault((X509Extension x) => x.Oid.Value == oidValue);
	}
}

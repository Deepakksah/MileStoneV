using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class SelectedCertificate : ExtensibleDataObject, ICertificate
{
	[DataMember(Order = 0)]
	public string FriendlyName { get; set; }

	[DataMember(Order = 1)]
	public string IssuerName { get; set; }

	[DataMember(Order = 2)]
	public string Subject { get; set; }

	[DataMember(Order = 3)]
	public bool HasPrivateKey { get; set; }

	[DataMember(Order = 4)]
	public string SubjectName { get; set; }

	[DataMember(Order = 5)]
	public DateTime ValidFrom { get; set; }

	[DataMember(Order = 6)]
	public DateTime ValidUntil { get; set; }

	[DataMember(Order = 7)]
	public string Thumbprint { get; set; }

	[DataMember(Order = 8)]
	public bool CertificateAuthority { get; set; }

	[DataMember(Order = 9)]
	public bool ServerAuthentication { get; set; }

	[DataMember(Order = 10)]
	public bool ClientAuthentication { get; set; }

	public X509Certificate2 X509Certificate => null;
}

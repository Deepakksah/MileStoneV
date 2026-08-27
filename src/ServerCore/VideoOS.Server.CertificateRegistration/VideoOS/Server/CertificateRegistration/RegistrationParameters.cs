using System;
using System.Xml;

namespace VideoOS.Server.CertificateRegistration;

public class RegistrationParameters
{
	public XmlDocument ServerConfigXml { get; set; }

	public XmlElement AuthorizationServerAddressElement { get; set; }

	public XmlElement ClientIdElement { get; set; }

	public XmlElement ServerAddressElement { get; set; }

	public string ServerConfigFileName { get; set; }

	public Uri AuthorizationServerAddress { get; set; }

	public Guid ClientId { get; set; }
}

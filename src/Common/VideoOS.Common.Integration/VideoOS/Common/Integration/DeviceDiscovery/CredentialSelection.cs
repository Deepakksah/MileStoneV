using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class CredentialSelection
{
	[XmlElement("Selected")]
	public bool Selected { get; set; }

	[XmlElement("Credential")]
	public Credential Credential { get; set; }

	public CredentialSelection()
	{
		Selected = false;
		Credential = new Credential();
	}

	public CredentialSelection(bool selected, Credential credential)
	{
		Selected = selected;
		Credential = credential;
	}
}

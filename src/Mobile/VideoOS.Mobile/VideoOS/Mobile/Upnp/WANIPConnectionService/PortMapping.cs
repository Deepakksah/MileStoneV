using System.Xml.Serialization;

namespace VideoOS.Mobile.Upnp.WANIPConnectionService;

public class PortMapping
{
	[XmlElement(Namespace = "", ElementName = "NewRemoteHost")]
	public string NewRemoteHost { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewExternalPort")]
	public ushort NewExternalPort { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewProtocol")]
	public string NewProtocol { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewInternalPort")]
	public ushort NewInternalPort { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewInternalClient")]
	public string NewInternalClient { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewEnabled")]
	public int NewEnabled
	{
		get
		{
			return Enabled ? 1 : 0;
		}
		set
		{
			Enabled = value == 1;
		}
	}

	[XmlElement(Namespace = "", ElementName = "NewPortMappingDescription")]
	public string NewPortMappingDescription { get; set; }

	[XmlElement(Namespace = "", ElementName = "NewLeaseDuration")]
	public uint NewLeaseDuration { get; set; }

	[XmlIgnore]
	public bool Enabled { get; set; }

	public PortMapping()
	{
		NewRemoteHost = "*";
		NewProtocol = "TCP";
		NewInternalClient = "";
		Enabled = true;
		NewPortMappingDescription = "";
		NewLeaseDuration = 0u;
	}

	protected PortMapping(PortMapping portMapping)
	{
		NewRemoteHost = portMapping.NewRemoteHost;
		NewProtocol = portMapping.NewProtocol;
		NewInternalClient = portMapping.NewInternalClient;
		Enabled = portMapping.Enabled;
		NewPortMappingDescription = portMapping.NewPortMappingDescription;
		NewLeaseDuration = portMapping.NewLeaseDuration;
		NewExternalPort = portMapping.NewExternalPort;
		NewInternalPort = portMapping.NewInternalPort;
	}
}

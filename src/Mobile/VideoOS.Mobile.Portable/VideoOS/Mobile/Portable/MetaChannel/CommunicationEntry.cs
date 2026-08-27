using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.MetaChannel;

[XmlRoot(ElementName = "Communication")]
public class CommunicationEntry
{
	public const string ConnectionIdXmlNode = "ConnectionId";

	public const string CommandsXmlNode = "Command";

	public const string EncryptedCommandXmlNode = "EncryptedCommand";

	[JsonProperty("ConnectionId")]
	[XmlElement(ElementName = "ConnectionId", DataType = "string")]
	public string ConnectionId { get; set; }

	[JsonProperty("Command")]
	[XmlElement(ElementName = "Command")]
	public Command Command { get; set; }

	[JsonProperty("EncryptedCommand")]
	[XmlElement(ElementName = "EncryptedCommand")]
	public string EncryptedCommand { get; set; }

	public CommunicationEntry()
	{
		Command = new Command();
	}
}

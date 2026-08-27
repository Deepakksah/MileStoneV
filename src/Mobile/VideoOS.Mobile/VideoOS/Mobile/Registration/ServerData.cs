using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Registration;

[XmlRoot(ElementName = "ServerData")]
public class ServerData
{
	public static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ServerData));

	[XmlAttribute(AttributeName = "ComputerName")]
	public string ComputerName { get; set; }

	[XmlAttribute(AttributeName = "HttpPort")]
	public int HttpPort { get; set; }

	[XmlAttribute(AttributeName = "HttpSecurePort")]
	public int HttpSecurePort { get; set; }

	[XmlAttribute(AttributeName = "Version")]
	public string ServerVersion { get; set; }

	[XmlAttribute(AttributeName = "S")]
	public string S { get; set; }

	[XmlAttribute(AttributeName = "T")]
	public string T { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is ServerData)
		{
			ServerData serverData = obj as ServerData;
			if (ComputerName == serverData.ComputerName && HttpPort == serverData.HttpPort && HttpSecurePort == serverData.HttpSecurePort && ServerVersion == serverData.ServerVersion && S == serverData.S)
			{
				return T == serverData.T;
			}
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ComputerName.GetHashCode() ^ HttpPort ^ HttpSecurePort ^ ServerVersion.GetHashCode() ^ S.GetHashCode() ^ T.GetHashCode();
	}

	public string ToXml()
	{
		return ToXml(Serializer);
	}

	public string ToXml(XmlSerializer serializer)
	{
		MemoryStream memoryStream = new MemoryStream();
		serializer.Serialize(memoryStream, this);
		return Encoding.UTF8.GetString(memoryStream.ToArray(), 0, (int)memoryStream.Length);
	}

	public static ServerData FromXml(string xmlServerData)
	{
		return FromXml(xmlServerData, Serializer);
	}

	public static ServerData FromXml(string xmlServerData, XmlSerializer serializer)
	{
		using MemoryStream input = new MemoryStream(Encoding.UTF8.GetBytes(xmlServerData));
		using XmlReader xmlReader = XmlReader.Create(input, SecurityXmlReaderSettings.Settings);
		return (ServerData)serializer.Deserialize(xmlReader);
	}
}

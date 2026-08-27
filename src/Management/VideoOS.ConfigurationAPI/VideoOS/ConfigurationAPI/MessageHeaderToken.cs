using System.ServiceModel.Channels;
using System.Xml;

namespace VideoOS.ConfigurationAPI;

public class MessageHeaderToken : MessageHeader
{
	private string _token;

	public override string Name => "Token";

	public override string Namespace => "";

	public string Token => _token;

	public MessageHeaderToken(string token)
	{
		_token = token;
	}

	protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
	{
		writer.WriteRaw(_token);
	}

	public static MessageHeaderToken ReadHeader(XmlDictionaryReader reader)
	{
		return new MessageHeaderToken(reader.ReadInnerXml());
	}
}

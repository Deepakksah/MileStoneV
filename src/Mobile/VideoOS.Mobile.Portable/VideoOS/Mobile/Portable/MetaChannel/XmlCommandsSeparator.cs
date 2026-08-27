using VideoOS.Mobile.Portable.MetaChannel.Serializers;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class XmlCommandsSeparator : CommandsSeparator
{
	public XmlCommandsSeparator(bool separateCommands = true)
		: base(new XmlMessageSerializer<CommunicationEntry>(), separateCommands)
	{
	}
}

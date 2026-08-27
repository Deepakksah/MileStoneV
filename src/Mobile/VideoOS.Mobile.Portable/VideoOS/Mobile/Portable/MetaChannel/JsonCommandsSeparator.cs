using VideoOS.Mobile.Portable.MetaChannel.Serializers;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class JsonCommandsSeparator : CommandsSeparator
{
	public JsonCommandsSeparator(bool separateCommands = true)
		: base(new JsonMessageSerializer<CommunicationEntry>(), separateCommands)
	{
	}
}

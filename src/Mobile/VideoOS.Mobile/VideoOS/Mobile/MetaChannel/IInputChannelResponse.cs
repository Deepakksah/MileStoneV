using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.MetaChannel;

public interface IInputChannelResponse
{
	ChannelTypes ChannelType { get; }

	ICommunicationChannel CommunicationChannel { get; }
}

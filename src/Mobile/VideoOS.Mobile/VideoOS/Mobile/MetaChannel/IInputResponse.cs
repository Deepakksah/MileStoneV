using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.MetaChannel;

public interface IInputResponse
{
	ComChannelDelegates.OnEntryResponse OnResponse { get; }

	ChannelTypes ChannelType { get; }

	ICommunicationChannel CreateComChannel(string szChannelId);

	void DestroyCommChannel(string szChannelId);
}

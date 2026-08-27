namespace VideoOS.Mobile.MetaChannel;

public interface IInputConsumer
{
	ComChannelDelegates.OnCommunicationEntry OnNewMessage { get; }

	ComChannelDelegates.GetChannelEncryption ChannelEncryption { get; }

	void OnCommChannelDestruction(string szChannelId, bool initiatedByClient);
}

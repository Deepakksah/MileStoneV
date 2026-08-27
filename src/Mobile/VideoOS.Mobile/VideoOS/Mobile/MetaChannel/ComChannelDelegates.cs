using System;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.MetaChannel;

public class ComChannelDelegates
{
	public delegate void OnCommunicationEntry(CommunicationEntry tEntry, string tChannelID, IInputResponse iResponse);

	public delegate void OnEntryResponse(CommunicationEntry tEntry, bool isFinal);

	public delegate void OnCommunicationEntryInput(CommunicationEntry tEntry, string tChannelID, IInputChannelResponse iInputChannelResponse);

	public delegate Tuple<DHEncryption, bool> GetChannelEncryption(string channelId);
}

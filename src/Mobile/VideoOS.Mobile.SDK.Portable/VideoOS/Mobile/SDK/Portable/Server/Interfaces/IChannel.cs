using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IChannel
{
	Connection.OnReceivedResponse OnResponse { get; set; }

	Connection.ConnectionEncryptionProperties ConnectionEncryptionProperties { get; set; }

	bool IsSynchronized { get; }

	void SendCommandEntry(CommunicationEntry entry, string address = null);
}

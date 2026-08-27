using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnectionEndPoint
{
	ChannelTypes ChannelType { get; }

	string IpAddress { get; }

	uint Port { get; }
}

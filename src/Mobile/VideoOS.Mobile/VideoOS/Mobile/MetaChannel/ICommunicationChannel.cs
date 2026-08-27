using System.Net;

namespace VideoOS.Mobile.MetaChannel;

public interface ICommunicationChannel
{
	EndPoint ClientEndPoint { get; }

	EndPoint ClientRemotePoint { get; }

	IListenerControl GetInnerControlObject { get; }
}

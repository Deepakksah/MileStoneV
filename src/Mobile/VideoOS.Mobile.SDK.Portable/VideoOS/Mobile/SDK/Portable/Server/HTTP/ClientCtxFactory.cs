using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Net.WebSockets;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class ClientCtxFactory
{
	private static CommunicationType _communicationType;

	public static void Init(CommunicationType type)
	{
		_communicationType = type;
	}

	public static IClientContext Create(bool isSecured)
	{
		if (_communicationType == CommunicationType.WebSocket)
		{
			return new WsClient(isSecured);
		}
		return new HttpClient(isSecured);
	}
}

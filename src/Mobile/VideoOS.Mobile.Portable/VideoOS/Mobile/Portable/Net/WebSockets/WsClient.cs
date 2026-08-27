using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.Portable.Net.WebSockets;

public class WsClient : IClientContext
{
	private readonly IClientContext _implementer;

	public string Protocol => _implementer.Protocol;

	public WsClient(bool isSecured)
	{
		_implementer = new WsClientImplementer(isSecured);
	}

	public IRequest CreateRequest(string webAddress)
	{
		return _implementer.CreateRequest(webAddress);
	}
}

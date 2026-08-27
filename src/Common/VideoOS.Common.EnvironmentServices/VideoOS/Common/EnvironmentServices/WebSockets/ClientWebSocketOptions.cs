using System.Net.WebSockets;

namespace VideoOS.Common.EnvironmentServices.WebSockets;

public class ClientWebSocketOptions : IClientWebSocketOptions
{
	private readonly System.Net.WebSockets.ClientWebSocketOptions _inner;

	public ClientWebSocketOptions(System.Net.WebSockets.ClientWebSocketOptions options)
	{
		_inner = options;
	}

	public void SetRequestHeader(string headerName, string headerValue)
	{
		_inner.SetRequestHeader(headerName, headerValue);
	}
}

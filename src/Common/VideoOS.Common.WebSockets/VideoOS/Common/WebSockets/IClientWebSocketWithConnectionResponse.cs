using System;
using System.Net.WebSockets;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public interface IClientWebSocketWithConnectionResponse : IClientWebSocket, IWebSocket, IDisposable
{
	WebSocketException Exception { get; }

	IHttpResponseMessage ConnectionResponse { get; }
}

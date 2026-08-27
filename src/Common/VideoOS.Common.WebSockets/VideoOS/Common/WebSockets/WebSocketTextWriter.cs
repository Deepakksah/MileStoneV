using System;
using System.Text;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketTextWriter : WebSocketWriter<string>
{
	private readonly Encoding _encoding;

	public WebSocketTextWriter(IWebSocket webSocket, Encoding encoding)
		: base(webSocket)
	{
		_encoding = encoding ?? throw new ArgumentNullException("encoding");
	}

	protected override byte[] FillBuffer(string message)
	{
		return _encoding.GetBytes(message);
	}
}

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketJsonWriter<T> : WebSocketWriter<T>
{
	private readonly Encoding _encoding;

	private readonly JsonSerializer _jsonSerializer;

	public WebSocketJsonWriter(IWebSocket webSocket, Encoding encoding)
		: base(webSocket)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		_encoding = encoding ?? throw new ArgumentNullException("encoding");
		_jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
		{
			MaxDepth = 128
		});
	}

	protected override byte[] FillBuffer(T message)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (StreamWriter streamWriter = new StreamWriter(memoryStream, _encoding))
		{
			_jsonSerializer.Serialize((TextWriter)streamWriter, (object)message);
		}
		return memoryStream.ToArray();
	}
}

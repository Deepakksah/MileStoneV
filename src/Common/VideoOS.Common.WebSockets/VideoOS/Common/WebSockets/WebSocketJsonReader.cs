using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketJsonReader<T> : WebSocketReader<T>
{
	private readonly JsonSerializer _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings
	{
		MaxDepth = 128
	});

	private readonly Encoding _encoding;

	public WebSocketJsonReader(IWebSocket webSocket, Encoding encoding)
		: base(webSocket)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		_encoding = encoding;
	}

	protected override Task<T> ReadFromStreamAsync(Stream stream, CancellationToken cancellationToken)
	{
		//IL_003b: Expected O, but got Unknown
		try
		{
			using StreamReader streamReader = new StreamReader(stream, _encoding);
			return Task.FromResult((T)_jsonSerializer.Deserialize((TextReader)streamReader, typeof(T)));
		}
		catch (JsonException ex)
		{
			JsonException innerException = ex;
			throw new WebSocketReaderException("Could not deserialize received data stream", (Exception)(object)innerException);
		}
	}
}

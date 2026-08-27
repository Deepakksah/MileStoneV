using System.IO;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketBinaryReader : WebSocketReader<byte[]>
{
	public WebSocketBinaryReader(IWebSocket webSocket)
		: base(webSocket)
	{
	}

	protected override async Task<byte[]> ReadFromStreamAsync(Stream stream, CancellationToken cancellationToken)
	{
		byte[] buffer = new byte[stream.Length];
		await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(continueOnCapturedContext: false);
		return buffer;
	}
}

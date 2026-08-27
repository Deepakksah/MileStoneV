using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.EnvironmentServices.WebSockets;

namespace VideoOS.Common.WebSockets;

public class WebSocketTextReader : WebSocketReader<string>
{
	private readonly Encoding _encoding;

	public WebSocketTextReader(IWebSocket webSocket, Encoding encoding)
		: base(webSocket)
	{
		_encoding = encoding;
	}

	protected override async Task<string> ReadFromStreamAsync(Stream stream, CancellationToken cancellationToken)
	{
		using StreamReader reader = new StreamReader(stream, _encoding);
		return await reader.ReadToEndAsync().ConfigureAwait(continueOnCapturedContext: false);
	}
}

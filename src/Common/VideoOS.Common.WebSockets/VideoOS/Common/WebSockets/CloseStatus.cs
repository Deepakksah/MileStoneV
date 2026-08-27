using System.Net.WebSockets;

namespace VideoOS.Common.WebSockets;

public class CloseStatus
{
	public WebSocketCloseStatus CloseStatusCode { get; }

	public string CloseDescription { get; }

	public static CloseStatus NormalClosure => new CloseStatus(WebSocketCloseStatus.NormalClosure, "");

	public CloseStatus(WebSocketCloseStatus closeStatus, string closeDescription)
	{
		CloseStatusCode = closeStatus;
		CloseDescription = closeDescription;
	}
}

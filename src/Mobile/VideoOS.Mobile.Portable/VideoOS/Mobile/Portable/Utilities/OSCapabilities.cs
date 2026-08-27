using System;
using System.Net.WebSockets;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public static class OSCapabilities
{
	private static readonly bool _isWSSupported;

	public static bool IsWebSocketsSupported => _isWSSupported;

	static OSCapabilities()
	{
		try
		{
			new ClientWebSocket();
			_isWSSupported = true;
		}
		catch (PlatformNotSupportedException)
		{
			Logger.Instance.Log(LogContent.Info, "OSCapabilities", "WebSockets are NOT supported");
		}
	}
}

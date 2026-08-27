using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public static class HttpChannelHelper
{
	public static string ContentTypeFromMessageType(MessageTypes messageType)
	{
		if (messageType != MessageTypes.Json)
		{
			return "text/xml";
		}
		return "text/json";
	}
}

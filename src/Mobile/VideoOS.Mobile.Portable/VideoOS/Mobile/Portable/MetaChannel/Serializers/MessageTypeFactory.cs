using System;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.Serializers;

public static class MessageTypeFactory
{
	public static T Create<T>(string dataType, Func<T> onJson, Func<T> onXml)
	{
		if (!string.IsNullOrWhiteSpace(dataType) && dataType.Contains("json"))
		{
			return onJson();
		}
		return onXml();
	}

	[ExcludeFromCodeCoverage]
	public static MessageTypes GetMessageType(string dataType)
	{
		return Create(dataType, () => MessageTypes.Json, () => MessageTypes.Xml);
	}
}

using System;

namespace VideoOS.Mobile.Portable.MetaChannel.Serializers;

public static class MessageSerializerFactory
{
	public static IMessageSerializer<T> CreateSerializer<T>(string dataType)
	{
		return MessageTypeFactory.Create(dataType, (Func<IMessageSerializer<T>>)(() => new JsonMessageSerializer<T>()), (Func<IMessageSerializer<T>>)(() => new XmlMessageSerializer<T>()));
	}
}

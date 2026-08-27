using System;
using VideoOS.Mobile.Portable.MetaChannel.Serializers;

namespace VideoOS.Mobile.Portable.MetaChannel;

public static class CommandSeparatorFactory
{
	public static CommandsSeparator CreateSeparator(string dataType)
	{
		return MessageTypeFactory.Create(dataType, (Func<CommandsSeparator>)(() => new JsonCommandsSeparator()), (Func<CommandsSeparator>)(() => new XmlCommandsSeparator()));
	}
}

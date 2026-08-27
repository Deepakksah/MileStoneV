using System;
using System.Linq;
using System.Text;

namespace VideoOS.Mobile.Http;

public static class StringBuilderExtension
{
	public static StringBuilder AppendJsonElement(this StringBuilder builder, string element)
	{
		builder.Append("'");
		element = element ?? string.Empty;
		string[] array = element.Split(new string[2] { "'", "'" }, StringSplitOptions.None);
		element = (array.Any() ? array[0] : string.Empty);
		builder.Append(element);
		builder.Append("'");
		return builder;
	}
}

using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Proxy.RestApi.ConfigApiRelated;

public static class JsonConvertUtilities
{
	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public static string CheckConvertPath(string text)
	{
		if (text == null)
		{
			return text;
		}
		int num = text.IndexOf('[');
		if (num > 0)
		{
			int num2 = text.IndexOf(']', num + 1);
			if (num2 - num == 37)
			{
				num--;
				while (num > 0 && char.IsLetter(text[num]))
				{
					num--;
				}
				if (num != 0)
				{
					num++;
				}
				num2++;
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text.Substring(num, num2 - num));
				string text2 = NameConvert.ItemTypeToResourcePlural(configurationItemPath.ItemType) + "/" + configurationItemPath.Id.ToLowerInvariant();
				string text3 = text.Substring(0, num);
				string text4 = text.Substring(num2);
				text = text3 + text2 + text4;
			}
		}
		return text;
	}
}

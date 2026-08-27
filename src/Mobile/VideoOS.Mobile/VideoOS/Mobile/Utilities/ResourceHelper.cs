using System.Resources;

namespace VideoOS.Mobile.Utilities;

public class ResourceHelper
{
	public static string GetValue(ResourceManager resources, string resourceString, string suffix = null)
	{
		string text = resources?.GetString(resourceString);
		if (!string.IsNullOrWhiteSpace(text))
		{
			text = text + " " + suffix;
		}
		return text;
	}
}

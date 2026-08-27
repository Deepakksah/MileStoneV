using System.Collections.Specialized;
using System.Globalization;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

public class ImageSettingsConfig : IConfigSection
{
	public const string NameMaxWidth = "MaxWidth";

	public const string NameMaxHeight = "MaxHeight";

	public int MaxWidth { get; private set; } = 1280;

	public int MaxHeight { get; private set; } = 720;

	public string SectionName => "ImageSettings";

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			MaxWidth = section["MaxWidth"].Sanitized().ToInt32();
		}
		catch
		{
			result = false;
		}
		try
		{
			MaxHeight = section["MaxHeight"].Sanitized().ToInt32();
		}
		catch
		{
			result = false;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["MaxWidth"] = MaxWidth.ToString(CultureInfo.InvariantCulture);
		}
		catch
		{
			result = false;
		}
		try
		{
			serverSection["MaxHeight"] = MaxHeight.ToString(CultureInfo.InvariantCulture);
		}
		catch
		{
			result = false;
		}
		return result;
	}
}

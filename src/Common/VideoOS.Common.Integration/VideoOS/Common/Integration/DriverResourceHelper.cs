using System.Globalization;
using System.Resources;

namespace VideoOS.Common.Integration;

public static class DriverResourceHelper
{
	private static ResourceManager resourceMan;

	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.DriverResources", typeof(DriverResourceHelper).Assembly)
				{
					IgnoreCase = true
				};
			}
			return resourceMan;
		}
	}

	public static string GetDisplayNameByDisplayNameReferenceId(string displayNameReferenceId, CultureInfo culture)
	{
		string text = ((displayNameReferenceId == null) ? "" : ResourceManager.GetString(displayNameReferenceId, culture));
		if (text == null)
		{
			text = string.Empty;
		}
		return text;
	}

	public static string GetDisplayNameByDisplayNameReferenceId(string displayNameReferenceId)
	{
		return GetDisplayNameByDisplayNameReferenceId(displayNameReferenceId, CultureInfo.CurrentUICulture);
	}
}

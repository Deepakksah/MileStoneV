using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Utilities.Formatting;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class FormattingResources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Utilities.Formatting.FormattingResources", typeof(FormattingResources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string BytesFormatBytes => ResourceManager.GetString("BytesFormatBytes", resourceCulture);

	public static string BytesFormatOneByte => ResourceManager.GetString("BytesFormatOneByte", resourceCulture);

	public static string BytesFormatOneDigit => ResourceManager.GetString("BytesFormatOneDigit", resourceCulture);

	public static string BytesFormatTwoDigits => ResourceManager.GetString("BytesFormatTwoDigits", resourceCulture);

	public static string BytesFormatZeroDigits => ResourceManager.GetString("BytesFormatZeroDigits", resourceCulture);

	public static string TimeFormatDays => ResourceManager.GetString("TimeFormatDays", resourceCulture);

	public static string TimeFormatHours => ResourceManager.GetString("TimeFormatHours", resourceCulture);

	public static string TimeFormatMinutes => ResourceManager.GetString("TimeFormatMinutes", resourceCulture);

	public static string TimeFormatOneDay => ResourceManager.GetString("TimeFormatOneDay", resourceCulture);

	public static string TimeFormatOneHour => ResourceManager.GetString("TimeFormatOneHour", resourceCulture);

	public static string TimeFormatOneMinute => ResourceManager.GetString("TimeFormatOneMinute", resourceCulture);

	public static string UnitExabyte => ResourceManager.GetString("UnitExabyte", resourceCulture);

	public static string UnitGigabyte => ResourceManager.GetString("UnitGigabyte", resourceCulture);

	public static string UnitKilobyte => ResourceManager.GetString("UnitKilobyte", resourceCulture);

	public static string UnitMegabyte => ResourceManager.GetString("UnitMegabyte", resourceCulture);

	public static string UnitPetabyte => ResourceManager.GetString("UnitPetabyte", resourceCulture);

	public static string UnitTerabyte => ResourceManager.GetString("UnitTerabyte", resourceCulture);

	public FormattingResources()
	{
	}
}

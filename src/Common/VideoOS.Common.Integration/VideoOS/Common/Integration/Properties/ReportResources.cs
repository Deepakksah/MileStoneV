using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Integration.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class ReportResources
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.ReportResources", typeof(ReportResources).Assembly);
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

	public static string ContactResellerAdditionalLicenses => ResourceManager.GetString("ContactResellerAdditionalLicenses", resourceCulture);

	public static string DeviceLicensesRequired => ResourceManager.GetString("DeviceLicensesRequired", resourceCulture);

	public static string DeviceListHeader => ResourceManager.GetString("DeviceListHeader", resourceCulture);

	public static string HeaderDeviceModelName => ResourceManager.GetString("HeaderDeviceModelName", resourceCulture);

	public static string HeaderDriverName => ResourceManager.GetString("HeaderDriverName", resourceCulture);

	public static string HeaderInformationLicenses => ResourceManager.GetString("HeaderInformationLicenses", resourceCulture);

	public static string HeaderMissingLicenses => ResourceManager.GetString("HeaderMissingLicenses", resourceCulture);

	public static string InformationTitle => ResourceManager.GetString("InformationTitle", resourceCulture);

	public static string PrintHint => ResourceManager.GetString("PrintHint", resourceCulture);

	public static string SingleLicensableDeviceCount => ResourceManager.GetString("SingleLicensableDeviceCount", resourceCulture);

	public static string SupportedDeviceListLink => ResourceManager.GetString("SupportedDeviceListLink", resourceCulture);

	public static string TotalMissingLicenses => ResourceManager.GetString("TotalMissingLicenses", resourceCulture);

	public static string TotalPurchasedLicenses => ResourceManager.GetString("TotalPurchasedLicenses", resourceCulture);

	public static string UserAssignedName => ResourceManager.GetString("UserAssignedName", resourceCulture);

	public static string WarningTitle => ResourceManager.GetString("WarningTitle", resourceCulture);

	public ReportResources()
	{
	}
}

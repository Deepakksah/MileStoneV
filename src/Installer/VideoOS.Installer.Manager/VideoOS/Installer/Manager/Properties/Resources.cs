using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Manager.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
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
				resourceMan = new ResourceManager("VideoOS.Installer.Manager.Properties.Resources", typeof(Resources).Assembly);
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

	public static string LicenseCheck_Expired => ResourceManager.GetString("LicenseCheck_Expired", resourceCulture);

	public static string LicenseCheck_ExpiredCaption => ResourceManager.GetString("LicenseCheck_ExpiredCaption", resourceCulture);

	public static string LicenseCheck_Invalid => ResourceManager.GetString("LicenseCheck_Invalid", resourceCulture);

	public static string LicenseCheck_InvalidCaption => ResourceManager.GetString("LicenseCheck_InvalidCaption", resourceCulture);

	public static string LicenseCheck_NotFound => ResourceManager.GetString("LicenseCheck_NotFound", resourceCulture);

	public static string LicenseCheck_NotFoundCaption => ResourceManager.GetString("LicenseCheck_NotFoundCaption", resourceCulture);

	public static string LicenseCheck_ProductInvalidLicense => ResourceManager.GetString("LicenseCheck_ProductInvalidLicense", resourceCulture);

	public Resources()
	{
	}
}

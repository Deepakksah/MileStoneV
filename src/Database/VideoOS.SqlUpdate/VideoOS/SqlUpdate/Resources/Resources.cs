using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.SqlUpdate.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
				resourceMan = new ResourceManager("VideoOS.SqlUpdate.Resources.Resources", typeof(Resources).Assembly);
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

	public static string DatabaseValidation_DatabaseCreationNotSupported => ResourceManager.GetString("DatabaseValidation_DatabaseCreationNotSupported", resourceCulture);

	public static string DatabaseValidation_DatabaseTypeDowngradeNotSupported => ResourceManager.GetString("DatabaseValidation_DatabaseTypeDowngradeNotSupported", resourceCulture);

	public static string DatabaseValidation_DatabaseVersionDowngradeNotSupported => ResourceManager.GetString("DatabaseValidation_DatabaseVersionDowngradeNotSupported", resourceCulture);

	public static string DatabaseValidation_VersionTooLow => ResourceManager.GetString("DatabaseValidation_VersionTooLow", resourceCulture);

	public Resources()
	{
	}
}

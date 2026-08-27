using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Setup.Properties;

[DebuggerNonUserCode]
[CompilerGenerated]
[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
				resourceMan = new ResourceManager("VideoOS.Installer.Setup.Properties.Resources", typeof(Resources).Assembly);
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

	public static string ErrorAnotherInstanceRunning => ResourceManager.GetString("ErrorAnotherInstanceRunning", resourceCulture);

	public static string ErrorFailedUnpack => ResourceManager.GetString("ErrorFailedUnpack", resourceCulture);

	public static string ErrorOccured => ResourceManager.GetString("ErrorOccured", resourceCulture);

	public static string ErrorPrereq => ResourceManager.GetString("ErrorPrereq", resourceCulture);

	public static string ErrorUnexpected => ResourceManager.GetString("ErrorUnexpected", resourceCulture);

	public static string ExternalArchiveHashMismatch => ResourceManager.GetString("ExternalArchiveHashMismatch", resourceCulture);

	public static string ExternalArchiveNotFound => ResourceManager.GetString("ExternalArchiveNotFound", resourceCulture);

	public static string ExternalArchiveReadError => ResourceManager.GetString("ExternalArchiveReadError", resourceCulture);

	public static string ExternalArchiveValidate => ResourceManager.GetString("ExternalArchiveValidate", resourceCulture);

	public static string Launching => ResourceManager.GetString("Launching", resourceCulture);

	public static string Preparing => ResourceManager.GetString("Preparing", resourceCulture);

	public static string Unpacking => ResourceManager.GetString("Unpacking", resourceCulture);

	public Resources()
	{
	}
}

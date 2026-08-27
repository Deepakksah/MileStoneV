using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Service.Properties;

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
				ResourceManager resourceManager = new ResourceManager("VideoOS.Server.Service.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = resourceManager;
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

	public static string ServerServiceDescription => ResourceManager.GetString("ServerServiceDescription", resourceCulture);

	public static string StatusConfigLoadFailed => ResourceManager.GetString("StatusConfigLoadFailed", resourceCulture);

	public static string StatusErrorDuringInitialization => ResourceManager.GetString("StatusErrorDuringInitialization", resourceCulture);

	public static string StatusFailedToReadConfig => ResourceManager.GetString("StatusFailedToReadConfig", resourceCulture);

	public static string StatusForcingShutdown => ResourceManager.GetString("StatusForcingShutdown", resourceCulture);

	public static string StatusInformationProviderFailed => ResourceManager.GetString("StatusInformationProviderFailed", resourceCulture);

	public static string StatusInformationProviderPortFailed => ResourceManager.GetString("StatusInformationProviderPortFailed", resourceCulture);

	public static string StatusNoAdminPrivileges => ResourceManager.GetString("StatusNoAdminPrivileges", resourceCulture);

	public static string StatusServerStarting => ResourceManager.GetString("StatusServerStarting", resourceCulture);

	public static string StatusShutdownKill => ResourceManager.GetString("StatusShutdownKill", resourceCulture);

	public static string StatusTerminateFatal => ResourceManager.GetString("StatusTerminateFatal", resourceCulture);

	public Resources()
	{
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.DataCollector.Service.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
				ResourceManager resourceManager = new ResourceManager("VideoOS.DataCollector.Service.Properties.Resources", typeof(Resources).Assembly);
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

	public static string DataCollectorServiceDescription => ResourceManager.GetString("DataCollectorServiceDescription", resourceCulture);

	public static string ErrorStoppingServiceCaption => ResourceManager.GetString("ErrorStoppingServiceCaption", resourceCulture);

	public static string ErrorStoppingServiceText => ResourceManager.GetString("ErrorStoppingServiceText", resourceCulture);

	public static string ServiceDescription => ResourceManager.GetString("ServiceDescription", resourceCulture);

	public static string StartupOptionsCaption => ResourceManager.GetString("StartupOptionsCaption", resourceCulture);

	public static string StartupOptionsText => ResourceManager.GetString("StartupOptionsText", resourceCulture);

	public static string StatusConfigLoadFailed => ResourceManager.GetString("StatusConfigLoadFailed", resourceCulture);

	public static string StatusDataCollectorStopped => ResourceManager.GetString("StatusDataCollectorStopped", resourceCulture);

	public static string StatusInformationProviderFailed => ResourceManager.GetString("StatusInformationProviderFailed", resourceCulture);

	public static string StatusInformationProviderPortFailed => ResourceManager.GetString("StatusInformationProviderPortFailed", resourceCulture);

	public static string StatusShutdownKill => ResourceManager.GetString("StatusShutdownKill", resourceCulture);

	public static string StatusShutdownNotice => ResourceManager.GetString("StatusShutdownNotice", resourceCulture);

	public static string StatusShutdownWaiting => ResourceManager.GetString("StatusShutdownWaiting", resourceCulture);

	public static string StatusTerminateFatal => ResourceManager.GetString("StatusTerminateFatal", resourceCulture);

	public Resources()
	{
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.LPR.Common.UI.LPRServiceStatus.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.LPR.Common.UI.LPRServiceStatus.Resources.Strings", typeof(Strings).Assembly);
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

	public static string ServiceStatusUserControlCpuUsageLabel => ResourceManager.GetString("ServiceStatusUserControlCpuUsageLabel", resourceCulture);

	public static string ServiceStatusUserControlLicensePlatesDetectedLabel => ResourceManager.GetString("ServiceStatusUserControlLicensePlatesDetectedLabel", resourceCulture);

	public static string ServiceStatusUserControlMemoryAvailableLabel => ResourceManager.GetString("ServiceStatusUserControlMemoryAvailableLabel", resourceCulture);

	public static string ServiceStatusUserControlServiceUpTimeLabel => ResourceManager.GetString("ServiceStatusUserControlServiceUpTimeLabel", resourceCulture);

	public static string ServiceStatusUserControlStatusLabel => ResourceManager.GetString("ServiceStatusUserControlStatusLabel", resourceCulture);

	public static string ServiceStatusUserControlVideoSourcesLabel => ResourceManager.GetString("ServiceStatusUserControlVideoSourcesLabel", resourceCulture);

	public Strings()
	{
	}
}

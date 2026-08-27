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
public class EvidenceLock
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.EvidenceLock", typeof(EvidenceLock).Assembly);
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

	public static string EvidenceLock_Default => ResourceManager.GetString("EvidenceLock_Default", resourceCulture);

	public static string EvidenceLock_TimeDay => ResourceManager.GetString("EvidenceLock_TimeDay", resourceCulture);

	public static string EvidenceLock_TimeDays => ResourceManager.GetString("EvidenceLock_TimeDays", resourceCulture);

	public static string EvidenceLock_TimeIndefinite => ResourceManager.GetString("EvidenceLock_TimeIndefinite", resourceCulture);

	public static string EvidenceLock_TimeMonth => ResourceManager.GetString("EvidenceLock_TimeMonth", resourceCulture);

	public static string EvidenceLock_TimeMonths => ResourceManager.GetString("EvidenceLock_TimeMonths", resourceCulture);

	public static string EvidenceLock_TimeUserDefined => ResourceManager.GetString("EvidenceLock_TimeUserDefined", resourceCulture);

	public static string EvidenceLock_TimeWeek => ResourceManager.GetString("EvidenceLock_TimeWeek", resourceCulture);

	public static string EvidenceLock_TimeWeeks => ResourceManager.GetString("EvidenceLock_TimeWeeks", resourceCulture);

	public static string EvidenceLock_TimeYear => ResourceManager.GetString("EvidenceLock_TimeYear", resourceCulture);

	public static string EvidenceLock_TimeYears => ResourceManager.GetString("EvidenceLock_TimeYears", resourceCulture);

	public EvidenceLock()
	{
	}
}

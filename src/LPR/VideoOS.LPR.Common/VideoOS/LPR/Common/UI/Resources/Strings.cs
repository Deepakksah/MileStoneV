using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.LPR.Common.UI.Resources;

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
				resourceMan = new ResourceManager("VideoOS.LPR.Common.UI.Resources.Strings", typeof(Strings).Assembly);
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

	public static string ServiceStatusCpuUsageText => ResourceManager.GetString("ServiceStatusCpuUsageText", resourceCulture);

	public static string ServiceStatusMemoryAvailableText => ResourceManager.GetString("ServiceStatusMemoryAvailableText", resourceCulture);

	public static string WizardWindow_ButtonCancel => ResourceManager.GetString("WizardWindow_ButtonCancel", resourceCulture);

	public static string WizardWindow_ButtonFinish => ResourceManager.GetString("WizardWindow_ButtonFinish", resourceCulture);

	public static string WizardWindow_ButtonNext => ResourceManager.GetString("WizardWindow_ButtonNext", resourceCulture);

	public static string WizardWindow_ButtonOk => ResourceManager.GetString("WizardWindow_ButtonOk", resourceCulture);

	public static string WizardWindow_ButtonPrevious => ResourceManager.GetString("WizardWindow_ButtonPrevious", resourceCulture);

	public Strings()
	{
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Help.UI.resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
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
				resourceMan = new ResourceManager("VideoOS.Common.Help.UI.resources.Strings", typeof(Strings).Assembly);
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

	public static string CloseButton => ResourceManager.GetString("CloseButton", resourceCulture);

	public static string CopyButtonTooltip => ResourceManager.GetString("CopyButtonTooltip", resourceCulture);

	public static string HelpNotAvailableDialogText1 => ResourceManager.GetString("HelpNotAvailableDialogText1", resourceCulture);

	public static string HelpNotAvailableDialogText2 => ResourceManager.GetString("HelpNotAvailableDialogText2", resourceCulture);

	public static string HelpNotAvailableDialogText3 => ResourceManager.GetString("HelpNotAvailableDialogText3", resourceCulture);

	public static string HelpNotAvailableDialogText4 => ResourceManager.GetString("HelpNotAvailableDialogText4", resourceCulture);

	public static string HelpNotAvailableDialogTitle => ResourceManager.GetString("HelpNotAvailableDialogTitle", resourceCulture);

	public Strings()
	{
	}
}

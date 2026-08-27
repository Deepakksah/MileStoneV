using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.TrayControllerControls.Properties;

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
				resourceMan = new ResourceManager("VideoOS.TrayControllerControls.Properties.Resources", typeof(Resources).Assembly);
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

	public static string StatusMessagesFormMenuItemCopyToClipboard => ResourceManager.GetString("StatusMessagesFormMenuItemCopyToClipboard", resourceCulture);

	public static string StatusMessagesFormMenuItemDetails => ResourceManager.GetString("StatusMessagesFormMenuItemDetails", resourceCulture);

	public static string StatusMessagesFormMessageColumn => ResourceManager.GetString("StatusMessagesFormMessageColumn", resourceCulture);

	public static string StatusMessagesFormTimeColumn => ResourceManager.GetString("StatusMessagesFormTimeColumn", resourceCulture);

	public Resources()
	{
	}
}

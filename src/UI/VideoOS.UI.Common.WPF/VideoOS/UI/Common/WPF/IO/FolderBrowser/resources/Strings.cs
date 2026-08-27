using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.IO.FolderBrowser.resources;

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
				resourceMan = new ResourceManager("VideoOS.UI.Common.WPF.IO.FolderBrowser.resources.Strings", typeof(Strings).Assembly);
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

	public static string DefaultFixedDiskName => ResourceManager.GetString("DefaultFixedDiskName", resourceCulture);

	public static string DefaultRemovableDiskName => ResourceManager.GetString("DefaultRemovableDiskName", resourceCulture);

	public static string DesktopDisplayName => ResourceManager.GetString("DesktopDisplayName", resourceCulture);

	public static string ErrorOpenDrive => ResourceManager.GetString("ErrorOpenDrive", resourceCulture);

	public static string ErrorOpenFolder => ResourceManager.GetString("ErrorOpenFolder", resourceCulture);

	public static string ErrorUnknown => ResourceManager.GetString("ErrorUnknown", resourceCulture);

	public static string LoadingDrives => ResourceManager.GetString("LoadingDrives", resourceCulture);

	public static string MyDocumentsDisplayName => ResourceManager.GetString("MyDocumentsDisplayName", resourceCulture);

	public Strings()
	{
	}
}

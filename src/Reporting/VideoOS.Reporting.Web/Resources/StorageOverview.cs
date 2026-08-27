using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Resources;

[GeneratedCode("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class StorageOverview
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
				ResourceManager resourceManager = new ResourceManager("Resources.StorageOverview", Assembly.Load("App_GlobalResources"));
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

	public static string ArrowTextArchive1 => ResourceManager.GetString("ArrowTextArchive1", resourceCulture);

	public static string ArrowTextArchive2 => ResourceManager.GetString("ArrowTextArchive2", resourceCulture);

	public static string ArrowTextDelete1 => ResourceManager.GetString("ArrowTextDelete1", resourceCulture);

	public static string ArrowTextDelete2 => ResourceManager.GetString("ArrowTextDelete2", resourceCulture);

	public static string B => ResourceManager.GetString("B", resourceCulture);

	public static string Blue => ResourceManager.GetString("Blue", resourceCulture);

	public static string bytes => ResourceManager.GetString("bytes", resourceCulture);

	public static string Dark => ResourceManager.GetString("Dark", resourceCulture);

	public static string DiskReadSpeed1 => ResourceManager.GetString("DiskReadSpeed1", resourceCulture);

	public static string DiskSize => ResourceManager.GetString("DiskSize", resourceCulture);

	public static string DiskWriteSpeed => ResourceManager.GetString("DiskWriteSpeed", resourceCulture);

	public static string FreeSpace => ResourceManager.GetString("FreeSpace", resourceCulture);

	public static string gb => ResourceManager.GetString("gb", resourceCulture);

	public static string IsDefault => ResourceManager.GetString("IsDefault", resourceCulture);

	public static string IsNotDefault => ResourceManager.GetString("IsNotDefault", resourceCulture);

	public static string kb => ResourceManager.GetString("kb", resourceCulture);

	public static string lblThemeType => ResourceManager.GetString("lblThemeType", resourceCulture);

	public static string Light => ResourceManager.GetString("Light", resourceCulture);

	public static string mb => ResourceManager.GetString("mb", resourceCulture);

	public static string MediaRecording => ResourceManager.GetString("MediaRecording", resourceCulture);

	public static string OtherData => ResourceManager.GetString("OtherData", resourceCulture);

	public static string Recording => ResourceManager.GetString("Recording", resourceCulture);

	public static string Refresh => ResourceManager.GetString("Refresh", resourceCulture);

	public static string tb => ResourceManager.GetString("tb", resourceCulture);

	public static string TitleDisks => ResourceManager.GetString("TitleDisks", resourceCulture);

	public static string TitleStorage => ResourceManager.GetString("TitleStorage", resourceCulture);

	public static string TotalSize => ResourceManager.GetString("TotalSize", resourceCulture);

	public StorageOverview()
	{
	}
}

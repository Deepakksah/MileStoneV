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
public class BuiltInGroupResources
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
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.BuiltInGroupResources", typeof(BuiltInGroupResources).Assembly);
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

	public static string AllCamerasDescription => ResourceManager.GetString("AllCamerasDescription", resourceCulture);

	public static string AllCamerasName => ResourceManager.GetString("AllCamerasName", resourceCulture);

	public static string AllInputsDescription => ResourceManager.GetString("AllInputsDescription", resourceCulture);

	public static string AllInputsName => ResourceManager.GetString("AllInputsName", resourceCulture);

	public static string AllMetadataDescription => ResourceManager.GetString("AllMetadataDescription", resourceCulture);

	public static string AllMetadataName => ResourceManager.GetString("AllMetadataName", resourceCulture);

	public static string AllMicrophonesDescription => ResourceManager.GetString("AllMicrophonesDescription", resourceCulture);

	public static string AllMicrophonesName => ResourceManager.GetString("AllMicrophonesName", resourceCulture);

	public static string AllOutputsDescription => ResourceManager.GetString("AllOutputsDescription", resourceCulture);

	public static string AllOutputsName => ResourceManager.GetString("AllOutputsName", resourceCulture);

	public static string AllSpeakersDescription => ResourceManager.GetString("AllSpeakersDescription", resourceCulture);

	public static string AllSpeakersName => ResourceManager.GetString("AllSpeakersName", resourceCulture);

	public BuiltInGroupResources()
	{
	}
}

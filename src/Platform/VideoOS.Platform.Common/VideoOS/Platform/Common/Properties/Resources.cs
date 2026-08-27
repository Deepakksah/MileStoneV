using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Platform.Common.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Platform.Common.Properties.Resources", typeof(Resources).Assembly);
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

	public static string Enum_ServerLogType_Audit => ResourceManager.GetString("Enum_ServerLogType_Audit", resourceCulture);

	public static string Enum_ServerLogType_Event => ResourceManager.GetString("Enum_ServerLogType_Event", resourceCulture);

	public static string Enum_ServerLogType_Rules => ResourceManager.GetString("Enum_ServerLogType_Rules", resourceCulture);

	public static string Enum_ServerLogType_System => ResourceManager.GetString("Enum_ServerLogType_System", resourceCulture);

	public Resources()
	{
	}
}

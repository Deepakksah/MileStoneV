using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Central.Util.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class StringResources
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
				resourceMan = new ResourceManager("VideoOS.Central.Util.Resources.StringResources", typeof(StringResources).Assembly);
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

	public static string SystemSound_Asterisk => ResourceManager.GetString("SystemSound_Asterisk", resourceCulture);

	public static string SystemSound_Beep => ResourceManager.GetString("SystemSound_Beep", resourceCulture);

	public static string SystemSound_Exclamation => ResourceManager.GetString("SystemSound_Exclamation", resourceCulture);

	public static string SystemSound_Hand => ResourceManager.GetString("SystemSound_Hand", resourceCulture);

	public static string SystemSound_Question => ResourceManager.GetString("SystemSound_Question", resourceCulture);

	public StringResources()
	{
	}
}

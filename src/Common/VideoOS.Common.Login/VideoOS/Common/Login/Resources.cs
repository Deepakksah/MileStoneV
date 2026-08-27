using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Login;

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
				resourceMan = new ResourceManager("VideoOS.Common.Login.Resources", typeof(Resources).Assembly);
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

	public static string LoginInvalidUsername => ResourceManager.GetString("LoginInvalidUsername", resourceCulture);

	public static string PasswordLabelSimple => ResourceManager.GetString("PasswordLabelSimple", resourceCulture);

	public static string PasswordLabelWindows => ResourceManager.GetString("PasswordLabelWindows", resourceCulture);

	public static string ServerTypeNameSimple => ResourceManager.GetString("ServerTypeNameSimple", resourceCulture);

	public static string ServerTypeNameWindows => ResourceManager.GetString("ServerTypeNameWindows", resourceCulture);

	public static string ServerTypeNameWindowsDefault => ResourceManager.GetString("ServerTypeNameWindowsDefault", resourceCulture);

	public static string UserLabelSimple => ResourceManager.GetString("UserLabelSimple", resourceCulture);

	public static string UserLabelWindows => ResourceManager.GetString("UserLabelWindows", resourceCulture);

	public Resources()
	{
	}
}

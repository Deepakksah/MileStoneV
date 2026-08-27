using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.UI.Mobile.Controls.Password;

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
				resourceMan = new ResourceManager("VideoOS.Mobile.UI.Mobile.Controls.Password.Resources", typeof(Resources).Assembly);
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

	public static string KeyManagementConfirmPassword => ResourceManager.GetString("KeyManagementConfirmPassword", resourceCulture);

	public static string KeyManagementDoNotAssignPassword => ResourceManager.GetString("KeyManagementDoNotAssignPassword", resourceCulture);

	public static string KeyManagementPassword => ResourceManager.GetString("KeyManagementPassword", resourceCulture);

	public static string KeyManagementPasswordPageHeader => ResourceManager.GetString("KeyManagementPasswordPageHeader", resourceCulture);

	public static string KeyManagementPasswordPageInfo => ResourceManager.GetString("KeyManagementPasswordPageInfo", resourceCulture);

	public static string KeyManagementPasswordPageInfoWarning => ResourceManager.GetString("KeyManagementPasswordPageInfoWarning", resourceCulture);

	public static string NewPasswordDoesNotMeetComplexityPolicy => ResourceManager.GetString("NewPasswordDoesNotMeetComplexityPolicy", resourceCulture);

	public static string NewPasswordEmpty => ResourceManager.GetString("NewPasswordEmpty", resourceCulture);

	public static string NewPasswordInvalid => ResourceManager.GetString("NewPasswordInvalid", resourceCulture);

	public static string NewPasswordPasswordsDoNotMatch => ResourceManager.GetString("NewPasswordPasswordsDoNotMatch", resourceCulture);

	public static string NewPasswordTooLong => ResourceManager.GetString("NewPasswordTooLong", resourceCulture);

	public static string NewPasswordTooShort => ResourceManager.GetString("NewPasswordTooShort", resourceCulture);

	public static Bitmap Warning => (Bitmap)ResourceManager.GetObject("Warning", resourceCulture);

	public Resources()
	{
	}
}

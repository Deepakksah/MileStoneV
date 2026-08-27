using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.CoSQLExprAdapter.Properties;

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
				resourceMan = new ResourceManager("VideoOS.Installer.CoSQLExprAdapter.Properties.Resources", typeof(Resources).Assembly);
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

	public static string SQL_InstallError => ResourceManager.GetString("SQL_InstallError", resourceCulture);

	public static string SQL_InstallError_NotCompatible => ResourceManager.GetString("SQL_InstallError_NotCompatible", resourceCulture);

	public static string SQL_InstallError_SP_Missing => ResourceManager.GetString("SQL_InstallError_SP_Missing", resourceCulture);

	public static string SQL_InstallException => ResourceManager.GetString("SQL_InstallException", resourceCulture);

	public static string SQL_InstallOK => ResourceManager.GetString("SQL_InstallOK", resourceCulture);

	public static string SQL_Running => ResourceManager.GetString("SQL_Running", resourceCulture);

	public static string SQL_StartInstallInfo => ResourceManager.GetString("SQL_StartInstallInfo", resourceCulture);

	public static string SQL_UnpackError => ResourceManager.GetString("SQL_UnpackError", resourceCulture);

	public static string SQL_UnpackInfo => ResourceManager.GetString("SQL_UnpackInfo", resourceCulture);

	public static string SQL_UnpackOK => ResourceManager.GetString("SQL_UnpackOK", resourceCulture);

	public static string SQL_VerifyConnection => ResourceManager.GetString("SQL_VerifyConnection", resourceCulture);

	[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
	public Resources()
	{
	}
}

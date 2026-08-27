using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Prerequisites.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
				resourceMan = new ResourceManager("VideoOS.Installer.Prerequisites.Properties.Resources", typeof(Resources).Assembly);
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

	public static string ErrorExtractingInstallerFiles => ResourceManager.GetString("ErrorExtractingInstallerFiles", resourceCulture);

	public static string Message_CheckingPrerequisistes => ResourceManager.GetString("Message_CheckingPrerequisistes", resourceCulture);

	public static string Message_CollectPrerequisites => ResourceManager.GetString("Message_CollectPrerequisites", resourceCulture);

	public static string Message_Unpacking => ResourceManager.GetString("Message_Unpacking", resourceCulture);

	public Resources()
	{
	}
}

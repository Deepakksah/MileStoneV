using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Vms.BackupRestore.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class BackupRestoreResources
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
				resourceMan = new ResourceManager("VideoOS.Vms.BackupRestore.Properties.BackupRestoreResources", typeof(BackupRestoreResources).Assembly);
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

	public static string BackupDefaultShareName => ResourceManager.GetString("BackupDefaultShareName", resourceCulture);

	public static string CreateShareExceptionAccessDenied => ResourceManager.GetString("CreateShareExceptionAccessDenied", resourceCulture);

	public static string CreateShareExceptionDuplicateShare => ResourceManager.GetString("CreateShareExceptionDuplicateShare", resourceCulture);

	public static string CreateShareExceptionInvalidName => ResourceManager.GetString("CreateShareExceptionInvalidName", resourceCulture);

	public static string CreateShareExceptionUnknown => ResourceManager.GetString("CreateShareExceptionUnknown", resourceCulture);

	public static string CreateShareExceptionUnknownDirectory => ResourceManager.GetString("CreateShareExceptionUnknownDirectory", resourceCulture);

	public static string DeleteShareAccessDeniedException => ResourceManager.GetString("DeleteShareAccessDeniedException", resourceCulture);

	public static string DeleteShareException => ResourceManager.GetString("DeleteShareException", resourceCulture);

	public static string DeleteShareFolderException => ResourceManager.GetString("DeleteShareFolderException", resourceCulture);

	public static string SqlServerUseLocalUserAccountException => ResourceManager.GetString("SqlServerUseLocalUserAccountException", resourceCulture);

	public BackupRestoreResources()
	{
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Admin.Tasks;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class SetupDatabaseTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.SetupDatabaseTask.Resources", typeof(SetupDatabaseTask_Resources).Assembly);
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

	public static string Construction_of_commandline_done_ => ResourceManager.GetString("Construction_of_commandline_done_", resourceCulture);

	public static string create_server_database_scriptfolder_replace => ResourceManager.GetString("create_server_database_scriptfolder_replace", resourceCulture);

	public static string create_server_database_scriptfolder_replace_allowOverwrite => ResourceManager.GetString("create_server_database_scriptfolder_replace_allowOverwrite", resourceCulture);

	public static string DbUpdate_exit_code => ResourceManager.GetString("DbUpdate_exit_code", resourceCulture);

	public static string DbUpdate_path => ResourceManager.GetString("DbUpdate_path", resourceCulture);

	public static string Exception_caught_in_DbUpdate => ResourceManager.GetString("Exception_caught_in_DbUpdate", resourceCulture);

	public static string ManagerApplicationName => ResourceManager.GetString("ManagerApplicationName", resourceCulture);

	public static string OemResource_Replacement_String => ResourceManager.GetString("OemResource_Replacement_String", resourceCulture);

	public static string The_specified_script_folder_does_not_exists => ResourceManager.GetString("The_specified_script_folder_does_not_exists", resourceCulture);

	public static string update_server_database_scriptfolder_replace => ResourceManager.GetString("update_server_database_scriptfolder_replace", resourceCulture);

	public static string ViewerClientApplicationName => ResourceManager.GetString("ViewerClientApplicationName", resourceCulture);

	public SetupDatabaseTask_Resources()
	{
	}
}

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
public class SetupMiscTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.SetupMiscTask.Resources", typeof(SetupMiscTask_Resources).Assembly);
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

	public static string Backup_share_folder => ResourceManager.GetString("Backup_share_folder", resourceCulture);

	public static string Backup_share_folder_created => ResourceManager.GetString("Backup_share_folder_created", resourceCulture);

	public static string Creating_backup_share_folder => ResourceManager.GetString("Creating_backup_share_folder", resourceCulture);

	public static string Database_name_is_null_or_empty => ResourceManager.GetString("Database_name_is_null_or_empty", resourceCulture);

	public static string Database_server_is_null_or_empty => ResourceManager.GetString("Database_server_is_null_or_empty", resourceCulture);

	public static string Error_updating_the_report_server_connection_string => ResourceManager.GetString("Error_updating_the_report_server_connection_string", resourceCulture);

	public static string Inserting_new_server_address => ResourceManager.GetString("Inserting_new_server_address", resourceCulture);

	public static string Management_server_install_directory_is_null_or_empty => ResourceManager.GetString("Management_server_install_directory_is_null_or_empty", resourceCulture);

	public static string Management_server_is_0_or_less => ResourceManager.GetString("Management_server_is_0_or_less", resourceCulture);

	public static string Report_server_settings_file_path_is_null_or_empty => ResourceManager.GetString("Report_server_settings_file_path_is_null_or_empty", resourceCulture);

	public static string Server_address_was_already_present_in_database => ResourceManager.GetString("Server_address_was_already_present_in_database", resourceCulture);

	public static string Setting_config_file_paths_for_both_32bit_and_64bit_registry => ResourceManager.GetString("Setting_config_file_paths_for_both_32bit_and_64bit_registry", resourceCulture);

	public static string Setting_server_address_in_database_if_it_is_not_present => ResourceManager.GetString("Setting_server_address_in_database_if_it_is_not_present", resourceCulture);

	public static string System_setting_was_not_found_in_the_database => ResourceManager.GetString("System_setting_was_not_found_in_the_database", resourceCulture);

	public static string Updated_the_Report_server_connection_string => ResourceManager.GetString("Updated_the_Report_server_connection_string", resourceCulture);

	public static string Updating_the_Report_server_connection_string => ResourceManager.GetString("Updating_the_Report_server_connection_string", resourceCulture);

	public SetupMiscTask_Resources()
	{
	}
}

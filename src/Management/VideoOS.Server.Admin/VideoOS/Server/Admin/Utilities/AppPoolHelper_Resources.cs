using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Server.Admin.Utilities;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class AppPoolHelper_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Utilities.AppPoolHelper.Resources", typeof(AppPoolHelper_Resources).Assembly);
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

	public static string Application_pool_created_Name => ResourceManager.GetString("Application_pool_created_Name", resourceCulture);

	public static string Application_pool_deleted => ResourceManager.GetString("Application_pool_deleted", resourceCulture);

	public static string Application_pool_not_found => ResourceManager.GetString("Application_pool_not_found", resourceCulture);

	public static string Application_pool_to_delete_is_null => ResourceManager.GetString("Application_pool_to_delete_is_null", resourceCulture);

	public static string AppPool_name_is_null => ResourceManager.GetString("AppPool_name_is_null", resourceCulture);

	public static string Creating_new_application_pool_Named => ResourceManager.GetString("Creating_new_application_pool_Named", resourceCulture);

	public static string Deleting_application_pool_Name => ResourceManager.GetString("Deleting_application_pool_Name", resourceCulture);

	public static string Deleting_existing_application_pool => ResourceManager.GetString("Deleting_existing_application_pool", resourceCulture);

	public static string Failed_in_CreateAppPool_with_the_following_exception => ResourceManager.GetString("Failed_in_CreateAppPool_with_the_following_exception", resourceCulture);

	public static string Metadata_path_does_not_include_W3SVCAppPools => ResourceManager.GetString("Metadata_path_does_not_include_W3SVCAppPools", resourceCulture);

	public static string Metadata_path_is_null => ResourceManager.GetString("Metadata_path_is_null", resourceCulture);

	public static string Setting_start_mode_on_the_application_pool => ResourceManager.GetString("Setting_start_mode_on_the_application_pool", resourceCulture);

	public static string Setting_the_managed_runtime_version_for_the_application_pool => ResourceManager.GetString("Setting_the_managed_runtime_version_for_the_application_pool", resourceCulture);

	public static string Setting_user_information_on_the_application_pool => ResourceManager.GetString("Setting_user_information_on_the_application_pool", resourceCulture);

	public static string Setting_WAMUser => ResourceManager.GetString("Setting_WAMUser", resourceCulture);

	public static string Setting_WAMUser_password_on_application_pool => ResourceManager.GetString("Setting_WAMUser_password_on_application_pool", resourceCulture);

	public static string The_application_pools_entry_is_null => ResourceManager.GetString("The_application_pools_entry_is_null", resourceCulture);

	public AppPoolHelper_Resources()
	{
	}
}

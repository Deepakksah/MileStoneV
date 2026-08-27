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
public class SetupIISTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.SetupIISTask.Resources", typeof(SetupIISTask_Resources).Assembly);
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

	public static string ASPNET_enabled_for_Net_20 => ResourceManager.GetString("ASPNET_enabled_for_Net_20", resourceCulture);

	public static string ASPNET_enabled_for_Net_40 => ResourceManager.GetString("ASPNET_enabled_for_Net_40", resourceCulture);

	public static string Created_the_App_Pools => ResourceManager.GetString("Created_the_App_Pools", resourceCulture);

	public static string Created_virtual_directory_Directory => ResourceManager.GetString("Created_virtual_directory_Directory", resourceCulture);

	public static string Granted_access_to_the_IIS_User => ResourceManager.GetString("Granted_access_to_the_IIS_User", resourceCulture);

	public static string IdentityServer => ResourceManager.GetString("IdentityServer", resourceCulture);

	public static string localhost => ResourceManager.GetString("localhost", resourceCulture);

	public static string ManagementServer => ResourceManager.GetString("ManagementServer", resourceCulture);

	public static string registerClientCallback_is_null => ResourceManager.GetString("registerClientCallback_is_null", resourceCulture);

	public static string Service_account_is_null => ResourceManager.GetString("Service_account_is_null", resourceCulture);

	public static string Starting_the_IIS_setup => ResourceManager.GetString("Starting_the_IIS_setup", resourceCulture);

	public static string The_directory_specified_for_the_virtual_directories_was_not_found => ResourceManager.GetString("The_directory_specified_for_the_virtual_directories_was_not_found", resourceCulture);

	public static string Url_Shortcut_File_folder_is_null_or_empty => ResourceManager.GetString("Url_Shortcut_File_folder_is_null_or_empty", resourceCulture);

	public static string Virtual_directory_parent_folder_is_null_or_empty => ResourceManager.GetString("Virtual_directory_parent_folder_is_null_or_empty", resourceCulture);

	public static string WCF_was_enbled => ResourceManager.GetString("WCF_was_enbled", resourceCulture);

	public static string Website_address_is_null_or_empty => ResourceManager.GetString("Website_address_is_null_or_empty", resourceCulture);

	public static string Website_id_is_null_or_empty => ResourceManager.GetString("Website_id_is_null_or_empty", resourceCulture);

	public static string Website_port_must_be_higher_than_0 => ResourceManager.GetString("Website_port_must_be_higher_than_0", resourceCulture);

	public SetupIISTask_Resources()
	{
	}
}

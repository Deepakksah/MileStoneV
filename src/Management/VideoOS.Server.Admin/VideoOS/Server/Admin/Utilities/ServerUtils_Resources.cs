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
public class ServerUtils_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Utilities.ServerUtils.Resources", typeof(ServerUtils_Resources).Assembly);
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

	public static string Adding_role_to_the_management_server => ResourceManager.GetString("Adding_role_to_the_management_server", resourceCulture);

	public static string Adding_Service_Account_to_Administrators_Role => ResourceManager.GetString("Adding_Service_Account_to_Administrators_Role", resourceCulture);

	public static string Adding_user_to_the_management_server => ResourceManager.GetString("Adding_user_to_the_management_server", resourceCulture);

	public static string Database_account_has_been_set => ResourceManager.GetString("Database_account_has_been_set", resourceCulture);

	public static string Error_adding_role => ResourceManager.GetString("Error_adding_role", resourceCulture);

	public static string Error_adding_user => ResourceManager.GetString("Error_adding_user", resourceCulture);

	public static string Error_connecting_to_server => ResourceManager.GetString("Error_connecting_to_server", resourceCulture);

	public static string Invalid_account => ResourceManager.GetString("Invalid_account", resourceCulture);

	public static string New_account_not_found => ResourceManager.GetString("New_account_not_found", resourceCulture);

	public static string NewAccountIsNull => ResourceManager.GetString("NewAccountIsNull", resourceCulture);

	public static string oldAccount => ResourceManager.GetString("oldAccount", resourceCulture);

	public static string OldAccountIsNull => ResourceManager.GetString("OldAccountIsNull", resourceCulture);

	public static string Service_account_changed => ResourceManager.GetString("Service_account_changed", resourceCulture);

	public static string Service_Account_Has_Been_Added_to_Administrators_Role => ResourceManager.GetString("Service_Account_Has_Been_Added_to_Administrators_Role", resourceCulture);

	public static string Service_namespace_security_has_been_set => ResourceManager.GetString("Service_namespace_security_has_been_set", resourceCulture);

	public static string ServiceAccountIsNull => ResourceManager.GetString("ServiceAccountIsNull", resourceCulture);

	public static string Setting_database_account_on_server => ResourceManager.GetString("Setting_database_account_on_server", resourceCulture);

	public static string Setting_the_service_account => ResourceManager.GetString("Setting_the_service_account", resourceCulture);

	public static string Setting_the_service_account_Old_user_0_New_user_1 => ResourceManager.GetString("Setting_the_service_account_Old_user_0_New_user_1", resourceCulture);

	public static string The_role_does_not_exist => ResourceManager.GetString("The_role_does_not_exist", resourceCulture);

	public static string User => ResourceManager.GetString("User", resourceCulture);

	public static string Validated_the_web_server => ResourceManager.GetString("Validated_the_web_server", resourceCulture);

	public ServerUtils_Resources()
	{
	}
}

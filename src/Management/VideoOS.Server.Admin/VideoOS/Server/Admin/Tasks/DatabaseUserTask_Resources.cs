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
public class DatabaseUserTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.DatabaseUserTask.Resources", typeof(DatabaseUserTask_Resources).Assembly);
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

	public static string Added_user_to_database_User => ResourceManager.GetString("Added_user_to_database_User", resourceCulture);

	public static string Database_name_is_null_or_empty => ResourceManager.GetString("Database_name_is_null_or_empty", resourceCulture);

	public static string Removed_user_from_database => ResourceManager.GetString("Removed_user_from_database", resourceCulture);

	public static string Server_name_is_null_or_empty => ResourceManager.GetString("Server_name_is_null_or_empty", resourceCulture);

	public static string Service_account_is_null => ResourceManager.GetString("Service_account_is_null", resourceCulture);

	public DatabaseUserTask_Resources()
	{
	}
}

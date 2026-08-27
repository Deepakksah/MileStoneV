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
public class CreateMediaDatabaseCertificateTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.CreateMediaDatabaseCertificateTask.Resources", typeof(CreateMediaDatabaseCertificateTask_Resources).Assembly);
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

	public static string CertificateIssuer_output => ResourceManager.GetString("CertificateIssuer_output", resourceCulture);

	public static string Exception_caught_in_CertificateIssuer => ResourceManager.GetString("Exception_caught_in_CertificateIssuer", resourceCulture);

	public static string Inserted_new_certificate_into_database => ResourceManager.GetString("Inserted_new_certificate_into_database", resourceCulture);

	public static string Media_database_certificate_allready_exists_in_database => ResourceManager.GetString("Media_database_certificate_allready_exists_in_database", resourceCulture);

	public static string privatekey_publickey => ResourceManager.GetString("privatekey_publickey", resourceCulture);

	public static string Unable_to_find_the_certificate_file_File => ResourceManager.GetString("Unable_to_find_the_certificate_file_File", resourceCulture);

	public CreateMediaDatabaseCertificateTask_Resources()
	{
	}
}

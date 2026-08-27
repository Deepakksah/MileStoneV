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
public class CreateLicenseAsymmetricKeyTask_Resources
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
				resourceMan = new ResourceManager("VideoOS.Server.Admin.Tasks.CreateLicenseAsymmetricKeyTask.Resources", typeof(CreateLicenseAsymmetricKeyTask_Resources).Assembly);
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

	public static string FailedToInsertKey => ResourceManager.GetString("FailedToInsertKey", resourceCulture);

	public static string KeyAlreadyExists => ResourceManager.GetString("KeyAlreadyExists", resourceCulture);

	public static string NewKeyInserted => ResourceManager.GetString("NewKeyInserted", resourceCulture);

	public CreateLicenseAsymmetricKeyTask_Resources()
	{
	}
}

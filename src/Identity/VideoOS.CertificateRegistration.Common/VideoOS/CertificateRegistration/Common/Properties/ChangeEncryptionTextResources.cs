using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.CertificateRegistration.Common.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class ChangeEncryptionTextResources
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
				resourceMan = new ResourceManager("VideoOS.CertificateRegistration.Common.Properties.ChangeEncryptionTextResources", typeof(ChangeEncryptionTextResources).Assembly);
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

	public static string CreateDefaultConfigFromCertificateFailed => ResourceManager.GetString("CreateDefaultConfigFromCertificateFailed", resourceCulture);

	public static string DeleteSslConfigurationFailed => ResourceManager.GetString("DeleteSslConfigurationFailed", resourceCulture);

	public static string EnforceSslConfigurationFailed => ResourceManager.GetString("EnforceSslConfigurationFailed", resourceCulture);

	public static string EnforceUrlReservationFailed => ResourceManager.GetString("EnforceUrlReservationFailed", resourceCulture);

	public static string FindServiceUserFailed => ResourceManager.GetString("FindServiceUserFailed", resourceCulture);

	public static string FindServiceUserSidFailed => ResourceManager.GetString("FindServiceUserSidFailed", resourceCulture);

	public ChangeEncryptionTextResources()
	{
	}
}

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Common.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
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
				resourceMan = new ResourceManager("VideoOS.Installer.Common.Properties.Resources", typeof(Resources).Assembly);
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

	public static string Action_ErorrStartService => ResourceManager.GetString("Action_ErorrStartService", resourceCulture);

	public static string Action_ErrorStopService => ResourceManager.GetString("Action_ErrorStopService", resourceCulture);

	public static string ErrorMessage_CertificateNotTrusted => ResourceManager.GetString("ErrorMessage_CertificateNotTrusted", resourceCulture);

	public static string ErrorMessage_CouldNotGetToken => ResourceManager.GetString("ErrorMessage_CouldNotGetToken", resourceCulture);

	public static string ErrorMessage_ServerAuthorizationFailed => ResourceManager.GetString("ErrorMessage_ServerAuthorizationFailed", resourceCulture);

	public static string KeyManagementErrorIncorrectPassword => ResourceManager.GetString("KeyManagementErrorIncorrectPassword", resourceCulture);

	public static string MessageBox_InvalidResourceSignatureCaption => ResourceManager.GetString("MessageBox_InvalidResourceSignatureCaption", resourceCulture);

	public static string MessageBox_InvalidResourceSignatureText => ResourceManager.GetString("MessageBox_InvalidResourceSignatureText", resourceCulture);

	public static string Temp_ErrorMessage_CertificateNotTrusted => ResourceManager.GetString("Temp_ErrorMessage_CertificateNotTrusted", resourceCulture);

	public Resources()
	{
	}
}

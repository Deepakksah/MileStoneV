using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.InstallerWizard;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Strings
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
				resourceMan = new ResourceManager("VideoOS.Mobile.InstallerWizard.Strings", typeof(Strings).Assembly);
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

	public static string CANNOTBESAME => ResourceManager.GetString("CANNOTBESAME", resourceCulture);

	public static string CertificateIsStillNotValid => ResourceManager.GetString("CertificateIsStillNotValid", resourceCulture);

	public static string CertificateWithoutPrivateKey => ResourceManager.GetString("CertificateWithoutPrivateKey", resourceCulture);

	public static string CHECKUSERPASSWORD => ResourceManager.GetString("CHECKUSERPASSWORD", resourceCulture);

	public static string COULDNOTLOGIN => ResourceManager.GetString("COULDNOTLOGIN", resourceCulture);

	public static string EnteredCertificateKasExpired => ResourceManager.GetString("EnteredCertificateKasExpired", resourceCulture);

	public static Bitmap error_16x16 => (Bitmap)ResourceManager.GetObject("error_16x16", resourceCulture);

	public static string InvalidCertificateCredentials => ResourceManager.GetString("InvalidCertificateCredentials", resourceCulture);

	public static string INVALIDCERTIFICATIONFILE => ResourceManager.GetString("INVALIDCERTIFICATIONFILE", resourceCulture);

	public static string INVALIDHTTPPORT => ResourceManager.GetString("INVALIDHTTPPORT", resourceCulture);

	public static string INVALIDHTTPSECUREPORT => ResourceManager.GetString("INVALIDHTTPSECUREPORT", resourceCulture);

	public static string INVALIDHTTPWCFPORT => ResourceManager.GetString("INVALIDHTTPWCFPORT", resourceCulture);

	public static string INVALIDURI => ResourceManager.GetString("INVALIDURI", resourceCulture);

	public static string INVALIDUSERNAME => ResourceManager.GetString("INVALIDUSERNAME", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType", resourceCulture);

	public static string MessageBox_UnableToVerifyAccountType_Caption => ResourceManager.GetString("MessageBox_UnableToVerifyAccountType_Caption", resourceCulture);

	public static string MUSTDEFINEONEPORT => ResourceManager.GetString("MUSTDEFINEONEPORT", resourceCulture);

	public static string REQUIREADMINISTRATIVERIGHTS => ResourceManager.GetString("REQUIREADMINISTRATIVERIGHTS", resourceCulture);

	public static string ServerCertificateControl_Error_CannotLoadStore => ResourceManager.GetString("ServerCertificateControl_Error_CannotLoadStore", resourceCulture);

	public static string ServerCertificateControl_Selection_CustomCert_Text => ResourceManager.GetString("ServerCertificateControl_Selection_CustomCert_Text", resourceCulture);

	public static string ServerCertificateControl_Selection_Invalid => ResourceManager.GetString("ServerCertificateControl_Selection_Invalid", resourceCulture);

	public static string ServerCertificateControl_Selection_NoCertAvailable_Text => ResourceManager.GetString("ServerCertificateControl_Selection_NoCertAvailable_Text", resourceCulture);

	public static string ServerCertificateControl_Selection_NoSelectionText => ResourceManager.GetString("ServerCertificateControl_Selection_NoSelectionText", resourceCulture);

	public static string SERVERNOTFOUND => ResourceManager.GetString("SERVERNOTFOUND", resourceCulture);

	public static string Temp_SERVERNOTFOUND => ResourceManager.GetString("Temp_SERVERNOTFOUND", resourceCulture);

	public static string VALIDATING => ResourceManager.GetString("VALIDATING", resourceCulture);

	public static string WizardPageControl_DetermineSystemAccountName_UserAccount => ResourceManager.GetString("WizardPageControl_DetermineSystemAccountName_UserAccount", resourceCulture);

	public static string WizardPageControl_LabelTitle_CustomMode => ResourceManager.GetString("WizardPageControl_LabelTitle_CustomMode", resourceCulture);

	public static string WizardPageControl_LabelTitle_TypicalMode => ResourceManager.GetString("WizardPageControl_LabelTitle_TypicalMode", resourceCulture);

	public static string WizardPagePorts_SpecifyPortsForCommunicationWithServer => ResourceManager.GetString("WizardPagePorts_SpecifyPortsForCommunicationWithServer", resourceCulture);

	public static string WizardPageServiceAccount_Headline => ResourceManager.GetString("WizardPageServiceAccount_Headline", resourceCulture);

	public static string WizardPageVmsAccount_Description => ResourceManager.GetString("WizardPageVmsAccount_Description", resourceCulture);

	public static string WizardPageVmsAccount_Headline => ResourceManager.GetString("WizardPageVmsAccount_Headline", resourceCulture);

	public static string WizardPageVmsAccount_RadioButton_BasicUser => ResourceManager.GetString("WizardPageVmsAccount_RadioButton_BasicUser", resourceCulture);

	public static string WizardPageVmsAccount_RadioButton_ServiceUser => ResourceManager.GetString("WizardPageVmsAccount_RadioButton_ServiceUser", resourceCulture);

	public Strings()
	{
	}
}

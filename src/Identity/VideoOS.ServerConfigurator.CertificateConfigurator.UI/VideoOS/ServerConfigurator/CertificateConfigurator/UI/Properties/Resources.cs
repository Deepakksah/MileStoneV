using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties;

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
				resourceMan = new ResourceManager("VideoOS.ServerConfigurator.CertificateConfigurator.UI.Properties.Resources", typeof(Resources).Assembly);
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

	public static string ApplyStatus_Success => ResourceManager.GetString("ApplyStatus_Success", resourceCulture);

	public static string ApplyStatus_SuccessInsufficientCredentials => ResourceManager.GetString("ApplyStatus_SuccessInsufficientCredentials", resourceCulture);

	public static string ApplyStatus_SuccessInvalidCredentials => ResourceManager.GetString("ApplyStatus_SuccessInvalidCredentials", resourceCulture);

	public static string ApplyStatus_SuccessRegistrationServerNotFound => ResourceManager.GetString("ApplyStatus_SuccessRegistrationServerNotFound", resourceCulture);

	public static string ApplyStatus_SuccessUnableToRegister => ResourceManager.GetString("ApplyStatus_SuccessUnableToRegister", resourceCulture);

	public static string ApplyStatus_UnspecifiedError => ResourceManager.GetString("ApplyStatus_UnspecifiedError", resourceCulture);

	public static string CertificateGroupControl_Details => ResourceManager.GetString("CertificateGroupControl_Details", resourceCulture);

	public static string CertificateGroupControl_EncryptionOff => ResourceManager.GetString("CertificateGroupControl_EncryptionOff", resourceCulture);

	public static string CertificateGroupControl_EncryptionOn => ResourceManager.GetString("CertificateGroupControl_EncryptionOn", resourceCulture);

	public static string CertificateView_DisplayInfo => ResourceManager.GetString("CertificateView_DisplayInfo", resourceCulture);

	public static string CertificateView_NoCertificateSelected => ResourceManager.GetString("CertificateView_NoCertificateSelected", resourceCulture);

	public static string CertificateView_SelectCertificate => ResourceManager.GetString("CertificateView_SelectCertificate", resourceCulture);

	public static string ConsistencyStatus_SecuritySchemeDoesNotMatch => ResourceManager.GetString("ConsistencyStatus_SecuritySchemeDoesNotMatch", resourceCulture);

	public static string ConsistencyStatus_ServerCertificateIsNotTrusted => ResourceManager.GetString("ConsistencyStatus_ServerCertificateIsNotTrusted", resourceCulture);

	public static string CredentialDialogCaption => ResourceManager.GetString("CredentialDialogCaption", resourceCulture);

	public static string CredentialDialogMessage => ResourceManager.GetString("CredentialDialogMessage", resourceCulture);

	public static string GeneralStatus_CouldNotStartServer => ResourceManager.GetString("GeneralStatus_CouldNotStartServer", resourceCulture);

	public static string GeneralStatus_CouldNotStopServer => ResourceManager.GetString("GeneralStatus_CouldNotStopServer", resourceCulture);

	public static string GeneralStatus_Success => ResourceManager.GetString("GeneralStatus_Success", resourceCulture);

	public static string GeneralStatus_UnexpectedError => ResourceManager.GetString("GeneralStatus_UnexpectedError", resourceCulture);

	public static string MessageBox_Apply => ResourceManager.GetString("MessageBox_Apply", resourceCulture);

	public static string MessageBox_ApplyCertificate_Text => ResourceManager.GetString("MessageBox_ApplyCertificate_Text", resourceCulture);

	public static string MessageBox_ApplyCertificates_Title => ResourceManager.GetString("MessageBox_ApplyCertificates_Title", resourceCulture);

	public static string MessageBox_Cancel => ResourceManager.GetString("MessageBox_Cancel", resourceCulture);

	public static string MessageBox_Change => ResourceManager.GetString("MessageBox_Change", resourceCulture);

	public static string MessageBox_ForceEditHostname_Text => ResourceManager.GetString("MessageBox_ForceEditHostname_Text", resourceCulture);

	public static string MessageBox_ForceEditHostname_Title => ResourceManager.GetString("MessageBox_ForceEditHostname_Title", resourceCulture);

	public static string MessageBox_Register => ResourceManager.GetString("MessageBox_Register", resourceCulture);

	public static string MessageBox_RegisterServers_Text => ResourceManager.GetString("MessageBox_RegisterServers_Text", resourceCulture);

	public static string MessageBox_RegisterServers_Title => ResourceManager.GetString("MessageBox_RegisterServers_Title", resourceCulture);

	public static string ProgressControl_DisablingEncryption => ResourceManager.GetString("ProgressControl_DisablingEncryption", resourceCulture);

	public static string ProgressControl_EnablingEncryption => ResourceManager.GetString("ProgressControl_EnablingEncryption", resourceCulture);

	public static string ProgressControl_RegisteringServers => ResourceManager.GetString("ProgressControl_RegisteringServers", resourceCulture);

	public static string ProgressControl_SomeServicesMayRestart => ResourceManager.GetString("ProgressControl_SomeServicesMayRestart", resourceCulture);

	public static string RegisterClientStatus_InsufficientCredentials => ResourceManager.GetString("RegisterClientStatus_InsufficientCredentials", resourceCulture);

	public static string RegisterClientStatus_InvalidCredentials => ResourceManager.GetString("RegisterClientStatus_InvalidCredentials", resourceCulture);

	public static string RegisterClientStatus_ManagementServerNotFound => ResourceManager.GetString("RegisterClientStatus_ManagementServerNotFound", resourceCulture);

	public static string RegisterClientStatus_Success => ResourceManager.GetString("RegisterClientStatus_Success", resourceCulture);

	public static string RegisterClientStatus_UnspecifiedError => ResourceManager.GetString("RegisterClientStatus_UnspecifiedError", resourceCulture);

	public static string RegisterService_ServiceRegistrationNotFound => ResourceManager.GetString("RegisterService_ServiceRegistrationNotFound", resourceCulture);

	public static string RegisterTabText => ResourceManager.GetString("RegisterTabText", resourceCulture);

	public static string ServerConfiguratorControl_Encryption => ResourceManager.GetString("ServerConfiguratorControl_Encryption", resourceCulture);

	public static string ServerConfiguratorControl_LanguageSelection => ResourceManager.GetString("ServerConfiguratorControl_LanguageSelection", resourceCulture);

	public static string ServerConfiguratorControl_RegisteringServers => ResourceManager.GetString("ServerConfiguratorControl_RegisteringServers", resourceCulture);

	public static string ServerEncryptionControl_Encryption => ResourceManager.GetString("ServerEncryptionControl_Encryption", resourceCulture);

	public static string ServerEncryptionControl_LearnMore => ResourceManager.GetString("ServerEncryptionControl_LearnMore", resourceCulture);

	public static string ServerEncryptionControl_Recommendation => ResourceManager.GetString("ServerEncryptionControl_Recommendation", resourceCulture);

	public static string ServerRegistrationControl_InvalidManagementServerAddress => ResourceManager.GetString("ServerRegistrationControl_InvalidManagementServerAddress", resourceCulture);

	public static string ServerRegistrationControl_LearnMore => ResourceManager.GetString("ServerRegistrationControl_LearnMore", resourceCulture);

	public static string ServerRegistrationControl_ManagementServerAddress => ResourceManager.GetString("ServerRegistrationControl_ManagementServerAddress", resourceCulture);

	public static string ServerRegistrationControl_Register => ResourceManager.GetString("ServerRegistrationControl_Register", resourceCulture);

	public static string ServerRegistrationControl_RegisteringServers => ResourceManager.GetString("ServerRegistrationControl_RegisteringServers", resourceCulture);

	public static string SetServerAddress_NotAllowedWhenManagementServerIsLocal => ResourceManager.GetString("SetServerAddress_NotAllowedWhenManagementServerIsLocal", resourceCulture);

	public static string SetServerAddress_UnspecifiedError => ResourceManager.GetString("SetServerAddress_UnspecifiedError", resourceCulture);

	public static string SettingsControl_ChooseLanguage => ResourceManager.GetString("SettingsControl_ChooseLanguage", resourceCulture);

	public static string SettingsControl_LanguageSelection => ResourceManager.GetString("SettingsControl_LanguageSelection", resourceCulture);

	public static string WaitCurtain_ApplyingConfiguration => ResourceManager.GetString("WaitCurtain_ApplyingConfiguration", resourceCulture);

	public Resources()
	{
	}
}

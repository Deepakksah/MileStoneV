using System;

namespace VideoOS.Mobile.InstallerWizard;

public class ParametersKeys
{
	public const string HTTPPort = "HTTPPORT";

	public const string HttpSecurePort = "HTTPSECUREPORT";

	public const string HttpSecureCertificatePath = "HTTPSCERTIFICATEPATH";

	public const string HttpSecureCertificatePassword = "HTTPSCERTIFICATEPASSWORD";

	public const string HttpSecureCertificateThumbprint = "HTTPSCERTIFICATETHUMBPRINT";

	public const string DeleteLogs = "DELETELOGS";

	public const string DeleteExports = "DELETEEXPORTS";

	public const string InstallerConfiguration = "INSTALLERCONFIGURATION";

	public const string Caller = "CALLER";

	public const string MosServiceUsername = "SERVICEACCOUNT";

	public const string MosServicePassword = "ENCRYPTEDPASSWORD";

	public const string MosServiceUserAccount = "USERACCOUNT";

	public const string MosVmsUseBasicUser = "MOSVMSUSEBASICUSER";

	public const string MosVmsUser = "MOSVMSUSER";

	public const string MosVmsPassword = "MOSVMSPASSWORD";

	public const string MosKeyManagementPassword = "KEYMANAGEMENTPASSWORD";

	public const string CallerWizardName = "{74CDB09C-BDEF-4FA6-ABF4-088DF4546501}";

	public static Guid CallerWizardId = new Guid("{74CDB09C-BDEF-4FA6-ABF4-088DF4546501}");

	public const string HttpsGenerateCertificateName = "{69B1AC1E-2DE5-4F81-8180-A868BE307D75}";

	public static Guid HttpsGenerateCertificate = new Guid("{69B1AC1E-2DE5-4F81-8180-A868BE307D75}");
}

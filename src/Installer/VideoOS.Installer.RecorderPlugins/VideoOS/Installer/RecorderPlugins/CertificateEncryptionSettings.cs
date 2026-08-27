namespace VideoOS.Installer.RecorderPlugins;

public class CertificateEncryptionSettings
{
	public bool EncryptionEnabled { get; set; } = true;

	public string CertificateHash { get; set; } = string.Empty;
}

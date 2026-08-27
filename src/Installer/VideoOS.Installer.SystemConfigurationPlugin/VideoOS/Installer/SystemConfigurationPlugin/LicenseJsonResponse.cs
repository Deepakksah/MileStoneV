namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicenseJsonResponse
{
	public LicenseData Data { get; set; }

	public int ErrorCode { get; set; }

	public object ErrorMessage { get; set; }

	public bool IsSuccessful { get; set; }
}

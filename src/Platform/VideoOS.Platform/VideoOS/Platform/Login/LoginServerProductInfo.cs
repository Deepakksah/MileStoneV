namespace VideoOS.Platform.Login;

public class LoginServerProductInfo
{
	public string ProductName { get; set; }

	public string ProductVersion { get; set; }

	public string MajorVersion { get; set; }

	public string MinorVersion { get; set; }

	public string ServiceVersion { get; set; }

	public int ServerSubType { get; set; }

	public LoginServerProductInfo()
	{
		ProductName = string.Empty;
		ProductVersion = string.Empty;
		MajorVersion = "0";
		MinorVersion = "0";
		ServiceVersion = "0";
		ServerSubType = 0;
	}
}

namespace VideoOS.Management.Common;

public class ServerCameraCredential
{
	public string UserName { get; private set; }

	public string Password { get; private set; }

	public ServerCameraCredential(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}
}

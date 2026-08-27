using VideoOS.Utilities.Security;

namespace VideoOS.GatewayService.Utilities;

public class SecureFolderWrapper : ISecureFolderWrapper
{
	private SecureFolder _secureFolder;

	public SecureFolderWrapper()
	{
		_secureFolder = new SecureFolder();
	}

	public void CreateSecureFolder(string folderPath, string accountValue)
	{
		_secureFolder.CreateSecureFolder(folderPath, accountValue);
	}

	public void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly)
	{
		_secureFolder.CreateSecureFolder(folderPath, accountValue, userAccountReadOnly);
	}

	public void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly, bool exclusive)
	{
		_secureFolder.CreateSecureFolder(folderPath, accountValue, userAccountReadOnly, exclusive);
	}
}

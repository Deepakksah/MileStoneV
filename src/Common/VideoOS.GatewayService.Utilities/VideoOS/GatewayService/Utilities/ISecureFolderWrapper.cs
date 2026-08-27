namespace VideoOS.GatewayService.Utilities;

public interface ISecureFolderWrapper
{
	void CreateSecureFolder(string folderPath, string accountValue);

	void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly);

	void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly, bool exclusive);
}

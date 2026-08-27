namespace VideoOS.Utilities.Security;

public interface ISecureFolder
{
	void CreateSecureFolder(string folderPath, string accountValue);

	void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly);
}

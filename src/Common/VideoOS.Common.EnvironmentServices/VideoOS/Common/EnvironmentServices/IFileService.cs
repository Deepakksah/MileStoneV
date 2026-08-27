using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace VideoOS.Common.EnvironmentServices;

public interface IFileService
{
	void Delete(string path);

	bool Exists(string path);

	void Move(string sourceFileName, string destinationFileName);

	void Move(string sourceFileName, string destinationFileName, bool overwrite);

	Stream Open(string path, FileMode mode, FileAccess access);

	Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

	string ReadAllText(string path);

	string ReadAllText(string path, Encoding encoding);

	void WriteAllText(string path, string contents);

	void WriteAllBytes(string path, byte[] bytes);

	IFileSecurity GetAccessControl(string path);

	IFileSecurity GetAccessControl(string path, AccessControlSections includeSections);

	void SetAccessControl(string path, IFileSecurity fileSecurity);
}

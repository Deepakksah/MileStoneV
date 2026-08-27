using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace VideoOS.Common.Crypto.Internal;

[CLSCompliant(true)]
public interface IFileService
{
	void Delete(string path);

	bool Exists(string path);

	void Move(string sourceFileName, string destinationFileName);

	Stream Open(string path, FileMode mode, FileAccess access);

	Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

	string ReadAllText(string path);

	string ReadAllText(string path, Encoding encoding);

	void WriteAllText(string path, string contents);

	void WriteAllBytes(string path, byte[] bytes);

	FileSecurity GetAccessControl(string path);

	FileSecurity GetAccessControl(string path, AccessControlSections controlSections);

	void SetAccessControl(string path, FileSecurity fileSecurity);

	void AddAccessRuleToFileSecurity(FileSecurity security, FileSystemAccessRule rule);
}

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace VideoOS.Common.Crypto.Internal;

[ExcludeFromCodeCoverage]
public class FileService : IFileService
{
	public Stream Open(string path, FileMode mode, FileAccess access)
	{
		return File.Open(path, mode, access);
	}

	public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
	{
		return File.Open(path, mode, access, share);
	}

	public bool Exists(string path)
	{
		return File.Exists(path);
	}

	public void Delete(string path)
	{
		File.Delete(path);
	}

	public void Move(string sourceFileName, string destinationFileName)
	{
		File.Move(sourceFileName, destinationFileName);
	}

	public string ReadAllText(string path)
	{
		return File.ReadAllText(path);
	}

	public string ReadAllText(string path, Encoding encoding)
	{
		return File.ReadAllText(path, encoding);
	}

	public void WriteAllText(string path, string contents)
	{
		File.WriteAllText(path, contents);
	}

	public void WriteAllBytes(string path, byte[] bytes)
	{
		File.WriteAllBytes(path, bytes);
	}

	public FileSecurity GetAccessControl(string path)
	{
		return File.GetAccessControl(path);
	}

	public FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
	{
		return File.GetAccessControl(path, includeSections);
	}

	public void SetAccessControl(string path, FileSecurity fileSecurity)
	{
		File.SetAccessControl(path, fileSecurity);
	}

	public void AddAccessRuleToFileSecurity(FileSecurity security, FileSystemAccessRule rule)
	{
		security.AddAccessRule(rule);
	}
}

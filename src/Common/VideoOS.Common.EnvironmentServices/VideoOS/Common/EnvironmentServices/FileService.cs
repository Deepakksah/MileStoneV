using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.AccessControl;
using System.Text;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
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

	public void Move(string sourceFileName, string destinationFileName, bool overwrite)
	{
		if (overwrite && File.Exists(destinationFileName))
		{
			File.Delete(destinationFileName);
		}
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

	public IFileSecurity GetAccessControl(string path)
	{
		return new FileSecurityWrapper(File.GetAccessControl(path));
	}

	public IFileSecurity GetAccessControl(string path, AccessControlSections includeSections)
	{
		return new FileSecurityWrapper(File.GetAccessControl(path, includeSections));
	}

	public void SetAccessControl(string path, IFileSecurity fileSecurity)
	{
		if (!(fileSecurity is FileSecurityWrapper fileSecurityWrapper))
		{
			throw new ArgumentException("fileSecurity must be implemented by the type FileSecurityWrapper");
		}
		File.SetAccessControl(path, fileSecurityWrapper.Inner);
	}
}

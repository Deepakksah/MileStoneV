using System.IO;
using System.Security.Cryptography;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Users;

public class ProtectedFile
{
	private static readonly long MaxSize = DirectoryHelper.MegaBytesToBytes(1L);

	private DataProtectionScope _dataProtectionScope;

	private string _fullFileName;

	public ProtectedFile(string fullFileName, DataProtectionScope dataProtectionScope)
	{
		_fullFileName = fullFileName;
		_dataProtectionScope = dataProtectionScope;
	}

	public T Load<T>()
	{
		if (new FileInfo(_fullFileName).Length > MaxSize)
		{
			throw new IOException("File size exceeds limit");
		}
		return Protector.Unprotect<T>(File.ReadAllBytes(_fullFileName), _dataProtectionScope);
	}

	public bool Save<T>(T data)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(_fullFileName));
		byte[] bytes = Protector.Protect(data, _dataProtectionScope);
		File.WriteAllBytes(_fullFileName, bytes);
		return true;
	}
}

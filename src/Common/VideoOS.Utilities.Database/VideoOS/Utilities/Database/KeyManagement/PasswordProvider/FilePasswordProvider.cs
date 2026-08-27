using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using VideoOS.Utilities.Security;

namespace VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

public class FilePasswordProvider : IPasswordProvider
{
	private readonly string _folderName;

	private readonly string _fileName;

	private readonly bool _userAccountReadOnly = true;

	private readonly ISecureFolder _secureFolder;

	public FilePasswordProvider(string folderName, string fileName, bool userAccountReadOnly = true)
		: this(folderName, fileName, new SecureFolder(), userAccountReadOnly)
	{
	}

	public FilePasswordProvider(string folderName, string fileName, ISecureFolder secureFolder, bool userAccountReadOnly = true)
	{
		if (string.IsNullOrEmpty(folderName))
		{
			throw new ArgumentNullException("folderName");
		}
		if (string.IsNullOrEmpty(fileName))
		{
			throw new ArgumentNullException("fileName");
		}
		_secureFolder = secureFolder ?? throw new ArgumentNullException("secureFolder");
		_folderName = folderName;
		_fileName = fileName;
		_userAccountReadOnly = userAccountReadOnly;
	}

	public string ReadPassword()
	{
		string path = BuildPath();
		byte[] encryptedData;
		try
		{
			encryptedData = File.ReadAllBytes(path);
		}
		catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is PathTooLongException || ex is DirectoryNotFoundException || ex is IOException || ex is FileNotFoundException || ex is NotSupportedException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileLocationError, "Invalid filepath.", ex);
		}
		catch (Exception ex2) when (ex2 is UnauthorizedAccessException || ex2 is SecurityException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.AccessDeniedToPasswordFile, "File and/or folder security settings are wrong.", ex2);
		}
		try
		{
			byte[] decoded = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.LocalMachine);
			return Decode(decoded);
		}
		catch (Exception ex3) when (ex3 is ArgumentNullException || ex3 is CryptographicException || ex3 is NotSupportedException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileCannotBeUnprotected, "Failed to unprotect data.", ex3);
		}
	}

	public IWritingSession StartWritingSession(string accountName)
	{
		if (string.IsNullOrEmpty(accountName))
		{
			throw new ArgumentNullException("accountName");
		}
		return new FileWritingSession(BuildFolderPath(), _fileName, accountName, _secureFolder, _userAccountReadOnly);
	}

	public void DeletePassword()
	{
		string path = BuildPath();
		try
		{
			File.Delete(path);
		}
		catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is PathTooLongException || ex is DirectoryNotFoundException || ex is IOException || ex is FileNotFoundException || ex is NotSupportedException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileLocationError, "Invalid filepath.", ex);
		}
		catch (Exception ex2) when (ex2 is UnauthorizedAccessException || ex2 is SecurityException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.AccessDeniedToPasswordFile, "File and/or folder security settings are wrong.", ex2);
		}
	}

	public bool HasPassword()
	{
		try
		{
			if (File.ReadAllBytes(BuildPath()).Length != 0)
			{
				return true;
			}
		}
		catch (Exception ex) when (ex is FileNotFoundException || ex is DirectoryNotFoundException)
		{
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (IOException ex3)
		{
			if ((ex3.HResult & 0xFFFF) == 32)
			{
				throw new KeyStorageException(KeyStorageErrorCode.PasswordFileInUse, "Password file in use", ex3);
			}
		}
		catch (SecurityException innerException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileSecurity, "Password file could not be accessed because of security", innerException);
		}
		catch (Exception innerException2)
		{
			throw new KeyStorageException(KeyStorageErrorCode.UnknownError, "Something went wrong when trying to check the password", innerException2);
		}
		return false;
	}

	private string BuildPath()
	{
		return Path.Combine(BuildFolderPath(), _fileName);
	}

	private string BuildFolderPath()
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _folderName);
	}

	private string Decode(byte[] decoded)
	{
		return Encoding.UTF8.GetString(decoded);
	}
}

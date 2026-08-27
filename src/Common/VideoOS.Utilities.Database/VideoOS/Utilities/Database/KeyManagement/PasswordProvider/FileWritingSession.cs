using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using VideoOS.Utilities.Security;

namespace VideoOS.Utilities.Database.KeyManagement.PasswordProvider;

public class FileWritingSession : IWritingSession, IDisposable
{
	private FileStream _fileStream;

	private readonly string _folderPath;

	private readonly string _accountName;

	private readonly ISecureFolder _secureFolder;

	private readonly bool _userAccountReadOnly;

	private bool disposedValue;

	public FileWritingSession(string folderPath, string filename, string accountName, ISecureFolder secureFolder, bool userAccountReadOnly = true)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			throw new ArgumentNullException("folderPath");
		}
		if (string.IsNullOrEmpty(filename))
		{
			throw new ArgumentNullException("filename");
		}
		if (string.IsNullOrEmpty(accountName))
		{
			throw new ArgumentNullException("accountName");
		}
		_secureFolder = secureFolder ?? throw new ArgumentNullException("secureFolder");
		_folderPath = folderPath;
		_accountName = accountName;
		_userAccountReadOnly = userAccountReadOnly;
		CreateFolderAndSetPermission();
		try
		{
			string path = Path.Combine(folderPath, filename);
			_fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		}
		catch (Exception ex) when (ex is PathTooLongException || ex is DirectoryNotFoundException || ex is ArgumentOutOfRangeException || ex is IOException || ex is NotSupportedException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileLocationError, "Could not create or write to file.", ex);
		}
		catch (Exception ex2) when (ex2 is UnauthorizedAccessException || ex2 is SecurityException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.AccessDeniedToPasswordFile, "File and/or folder security settings are wrong.", ex2);
		}
	}

	public void Write(string password)
	{
		byte[] array;
		try
		{
			array = ProtectedData.Protect(Encode(password), null, DataProtectionScope.LocalMachine);
		}
		catch (Exception ex) when (ex is ArgumentNullException || ex is CryptographicException || ex is NotSupportedException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileCannotBeProtected, "Failed to protect data.", ex);
		}
		_fileStream.Write(array, 0, array.Length);
	}

	private void CreateFolderAndSetPermission()
	{
		try
		{
			_secureFolder.CreateSecureFolder(_folderPath, _accountName, _userAccountReadOnly);
		}
		catch (FileAccessControlException innerException)
		{
			throw new KeyStorageException(KeyStorageErrorCode.PasswordFileLocationError, "Couldn't create secure folder. See inner exception for more details.", innerException);
		}
	}

	private byte[] Encode(string encodedText)
	{
		return Encoding.UTF8.GetBytes(encodedText);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (_fileStream != null)
			{
				_fileStream.Dispose();
			}
			disposedValue = true;
		}
	}

	~FileWritingSession()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

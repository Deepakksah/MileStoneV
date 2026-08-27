using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using VideoOS.Common.Crypto.Internal;

namespace VideoOS.Common.Crypto;

[CLSCompliant(true)]
public class FileSecretProvider : ISecretProvider
{
	private static byte[] entropy = new byte[16]
	{
		2, 8, 2, 6, 5, 1, 1, 9, 8, 6,
		0, 1, 9, 4, 1, 7
	};

	private string _tempFile;

	private string _permFile;

	private readonly string _tempFileName;

	private readonly string _permFileName;

	private IFileService _fileService;

	private IProtectDataService _protectDataService;

	public FileSecretProvider(string folder, string tempFileName, string permFileName)
		: this(folder, tempFileName, permFileName, new ProtectDataService(), new FileService())
	{
	}

	public FileSecretProvider(string folder, string tempFileName, string permFileName, IProtectDataService protectDataService, IFileService fileService)
	{
		if (folder == null)
		{
			throw new ArgumentNullException("folder");
		}
		if (string.IsNullOrWhiteSpace(tempFileName))
		{
			throw new ArgumentNullException("tempFileName");
		}
		if (string.IsNullOrWhiteSpace(permFileName))
		{
			throw new ArgumentNullException("permFileName");
		}
		if (protectDataService == null)
		{
			throw new ArgumentNullException("protectDataService");
		}
		if (fileService == null)
		{
			throw new ArgumentNullException("fileService");
		}
		if (!string.IsNullOrEmpty(Path.GetExtension(folder)))
		{
			throw new ArgumentException("Path must be without filename");
		}
		_fileService = fileService;
		_protectDataService = protectDataService;
		_tempFileName = tempFileName;
		_permFileName = permFileName;
		_tempFile = Path.Combine(folder, _tempFileName);
		_permFile = Path.Combine(folder, _permFileName);
	}

	public void DeleteTempSecret()
	{
		try
		{
			if (_fileService.Exists(_tempFile))
			{
				_fileService.Delete(_tempFile);
			}
		}
		catch (Exception innerException)
		{
			throw new RemoveFileSecretException("An error occurred during deleting temporary secret", innerException);
		}
	}

	public void DeletePermSecret()
	{
		try
		{
			if (_fileService.Exists(_permFile))
			{
				_fileService.Delete(_permFile);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new RemoveFileSecretException("An error occurred during deleting permanent secret", innerException);
		}
	}

	public byte[] DecryptSecret(byte[] secret, DataProtectionScope scope)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		try
		{
			return _protectDataService.Unprotect(secret, entropy, scope);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new SecretException("An error occurred during decrypting secret", innerException);
		}
	}

	public byte[] EncryptSecret(byte[] secret, DataProtectionScope scope)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		try
		{
			return _protectDataService.Protect(secret, entropy, scope);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new SecretException("An error occurred during encrypting secret", innerException);
		}
	}

	public byte[] ReadSecret()
	{
		return ReadFromFile(_permFile);
	}

	public void WriteSecret(byte[] secret)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		WriteToFile(secret, _permFile);
	}

	public void WriteSecret(byte[] secret, FileSystemAccessRule rule)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		WriteToFile(secret, _permFile, rule);
	}

	public byte[] ReadTempSecret()
	{
		if (!_fileService.Exists(_tempFile))
		{
			return null;
		}
		return ReadFromFile(_tempFile);
	}

	public void WriteTempSecret(byte[] secret)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		WriteToFile(secret, _tempFile);
	}

	public void WriteTempSecret(byte[] secret, FileSystemAccessRule rule)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		WriteToFile(secret, _tempFile, rule);
	}

	private byte[] ReadFromFile(string fileName)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		try
		{
			using Stream stream = _fileService.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, (int)stream.Length);
			return array;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (DirectoryNotFoundException innerException)
		{
			throw new SecretNotFoundException(innerException);
		}
		catch (FileNotFoundException innerException2)
		{
			throw new SecretNotFoundException(innerException2);
		}
		catch (Exception innerException3)
		{
			throw new SecretException("An error occurred during getting secret", innerException3);
		}
	}

	private void WriteToFile(byte[] secret, string fileName, FileSystemAccessRule rule)
	{
		WriteToFile(secret, fileName);
		try
		{
			FileSecurity accessControl = _fileService.GetAccessControl(fileName, AccessControlSections.Access);
			_fileService.AddAccessRuleToFileSecurity(accessControl, rule);
			_fileService.SetAccessControl(fileName, accessControl);
		}
		catch (FileNotFoundException)
		{
		}
	}

	private void WriteToFile(byte[] secret, string fileName)
	{
		if (secret == null)
		{
			throw new ArgumentNullException("secret");
		}
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		try
		{
			using Stream stream = _fileService.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
			stream.Write(secret, 0, secret.Length);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new SecretException("An error occurred during storing secret", innerException);
		}
	}
}

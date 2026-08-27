using System;
using System.IO;
using System.Security.Cryptography;
using VideoOS.Mobile.CommonUtilities;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Users;

public class BasicUserCredentialsStore
{
	private const string PasswordFolder = "Credentials";

	private const string PasswordFile = "DECFE722-6EB0-41DE-8F7B-C9271C29EF64.txt";

	private string _fullDirectoryName;

	private string _fullFileName;

	private ProtectedFile _protectedFile;

	public static BasicUserCredentialsStore Instance = new BasicUserCredentialsStore();

	private BasicUserCredentialsStore()
	{
		_fullDirectoryName = DirectoryHelper.BuildCommonApplicationDataFilePath("Credentials", OemResources.Instance.MobileServerProductNameWithoutCompany, OemResources.Instance.OemVendorProgramDataFolderName, createDir: false);
		_fullFileName = Path.Combine(_fullDirectoryName, "DECFE722-6EB0-41DE-8F7B-C9271C29EF64.txt");
		_protectedFile = new ProtectedFile(_fullFileName, DataProtectionScope.LocalMachine);
	}

	public ICredentials Load(bool reencrypt, Action<string> onError = null)
	{
		try
		{
			if (!File.Exists(_fullFileName))
			{
				return null;
			}
			ProtectedObject protectedObject = _protectedFile.Load<ProtectedObject>();
			Credentials credentials;
			if (protectedObject.ProtectionType == ProtectionType.None)
			{
				credentials = protectedObject.Data.ByteDeserialize<Credentials>();
				if (reencrypt)
				{
					ProtectedObject data = new ProtectedObject
					{
						Data = Protector.Protect(credentials, DataProtectionScope.CurrentUser),
						ProtectionType = ProtectionType.CurrentUser
					};
					_protectedFile.Save(data);
				}
			}
			else
			{
				credentials = Protector.Unprotect<Credentials>(protectedObject.Data, DataProtectionScope.CurrentUser);
			}
			return credentials;
		}
		catch (Exception ex)
		{
			onError?.Invoke(ex.Message);
			return null;
		}
	}

	public bool Save(ICredentials credentials, Action<string> onError = null)
	{
		try
		{
			ProtectedObject data = new ProtectedObject
			{
				Data = credentials.ByteSerialize(),
				ProtectionType = ProtectionType.None
			};
			return _protectedFile.Save(data);
		}
		catch (Exception ex)
		{
			onError?.Invoke(ex.Message);
			return false;
		}
	}

	public bool Delete(Action<string> onError = null)
	{
		try
		{
			Directory.Delete(_fullDirectoryName, recursive: true);
			return true;
		}
		catch (Exception ex)
		{
			onError?.Invoke(ex.Message);
			return false;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using VideoOS.Utilities.Security;
using VideoOS.Vms.BackupRestore.Properties;

namespace VideoOS.Vms.BackupRestore;

public static class DirectoryUtilities
{
	private static readonly SecurityIdentifier _networkServiceSid = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);

	private static readonly SecurityIdentifier _localServiceSid = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);

	private static readonly SecurityIdentifier _localSystemSid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);

	public static ShareResult GetShareResult(string shareResult)
	{
		if (Enum.GetNames(typeof(ShareResult)).Contains(shareResult, StringComparer.OrdinalIgnoreCase))
		{
			return (ShareResult)Enum.Parse(typeof(ShareResult), shareResult, ignoreCase: true);
		}
		return ShareResult.UnknownFailure;
	}

	public static ShareResult GetShareResult(uint shareResult)
	{
		string name = Enum.GetName(typeof(ShareResult), shareResult);
		if (name != null)
		{
			return (ShareResult)Enum.Parse(typeof(ShareResult), name, ignoreCase: true);
		}
		return ShareResult.UnknownFailure;
	}

	private static ShareResult CreateSharedFolder(string fullBackupFolderPath, string shareName, SecurityIdentifier[] userSecurityIdentifiers)
	{
		RemoveShare(shareName);
		if (Directory.Exists(fullBackupFolderPath) && !DeleteFolder(fullBackupFolderPath))
		{
			throw new DeleteShareFolderException(BackupRestoreResources.DeleteShareFolderException);
		}
		Directory.CreateDirectory(fullBackupFolderPath);
		ManagementClass managementClass = new ManagementClass("Win32_Share");
		ManagementBaseObject methodParameters = managementClass.GetMethodParameters("Create");
		methodParameters["Description"] = "Corporate backup file share description.";
		methodParameters["Name"] = shareName;
		methodParameters["Path"] = fullBackupFolderPath;
		methodParameters["Type"] = 0;
		methodParameters["MaximumAllowed"] = null;
		methodParameters["Password"] = null;
		List<ManagementObject> list = new List<ManagementObject>();
		foreach (SecurityIdentifier obj in userSecurityIdentifiers)
		{
			byte[] array = new byte[obj.BinaryLength];
			obj.GetBinaryForm(array, 0);
			ManagementObject managementObject = new ManagementClass(new ManagementPath("Win32_Trustee"), null);
			managementObject["SID"] = array;
			ManagementObject managementObject2 = new ManagementClass(new ManagementPath("Win32_Ace"), null);
			managementObject2["AccessMask"] = 2032127;
			managementObject2["AceFlags"] = AceFlags.ObjectInheritAce | AceFlags.ContainerInheritAce;
			managementObject2["AceType"] = AceType.AccessAllowed;
			managementObject2["Trustee"] = managementObject;
			list.Add(managementObject2);
		}
		ManagementObject managementObject3 = new ManagementClass(new ManagementPath("Win32_SecurityDescriptor"), null);
		managementObject3["ControlFlags"] = 4;
		managementObject3["DACL"] = list.ToArray();
		methodParameters["Access"] = managementObject3;
		return GetShareResult((uint)managementClass.InvokeMethod("Create", methodParameters, null).Properties["ReturnValue"].Value);
	}

	public static void CreateSharedFolderSqlServerAccount(string fullBackupFolderPath, string shareName, CorporateSqlData corporateSqlData)
	{
		if (corporateSqlData == null)
		{
			throw new ArgumentNullException("corporateSqlData");
		}
		SqlServerInfo sqlServerInfo = corporateSqlData.SqlServiceAccount();
		SecurityIdentifier securityIdentifier = GetSid(sqlServerInfo.SqlAccountName, sqlServerInfo.MachineName);
		if (securityIdentifier == null)
		{
			securityIdentifier = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
		}
		if (string.Compare(sqlServerInfo.MachineName, Environment.MachineName, StringComparison.OrdinalIgnoreCase) != 0)
		{
			if (securityIdentifier.Equals(_localSystemSid) || securityIdentifier.Equals(_localServiceSid))
			{
				throw new SqlServerUseLocalUserAccountException(BackupRestoreResources.SqlServerUseLocalUserAccountException);
			}
			if (securityIdentifier.Equals(_networkServiceSid))
			{
				securityIdentifier = GetSid(sqlServerInfo.MachineName + "$", sqlServerInfo.MachineName);
			}
			switch (CreateSharedFolder(fullBackupFolderPath, shareName, new SecurityIdentifier[1] { securityIdentifier }))
			{
			case ShareResult.AccessDenied:
				throw new CreateShareAccessDeniedException(BackupRestoreResources.CreateShareExceptionAccessDenied);
			case ShareResult.InvalidName:
				throw new CreateShareInvalidNameException(BackupRestoreResources.CreateShareExceptionInvalidName);
			case ShareResult.DuplicateShare:
				throw new CreateShareDuplicateShareException(BackupRestoreResources.CreateShareExceptionDuplicateShare);
			case ShareResult.UnknownDeviceOrDirectory:
				throw new CreateShareUnknownDirectoryException(BackupRestoreResources.CreateShareExceptionUnknownDirectory);
			default:
				throw new CreateShareUnknownErrorException(BackupRestoreResources.CreateShareExceptionUnknown);
			case ShareResult.Success:
				break;
			}
		}
		SecurityIdentifier account = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
		AddDirectorySecurity(fullBackupFolderPath, account, FileSystemRights.Modify, AccessControlType.Allow);
	}

	private static SecurityIdentifier GetSid(string userAccount, string userMachineName)
	{
		try
		{
			ParseIdentityName(userAccount, out var domain, out var accountName);
			NTAccount nTAccount = ((string.Compare(userMachineName, domain, StringComparison.OrdinalIgnoreCase) != 0) ? new NTAccount(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", domain, accountName)) : new NTAccount(accountName));
			return (SecurityIdentifier)nTAccount.Translate(typeof(SecurityIdentifier));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return null;
		}
	}

	private static void ParseIdentityName(string name, out string domain, out string accountName)
	{
		domain = null;
		accountName = null;
		int num = name.IndexOfAny(new char[2] { '\\', '/' });
		if (num < 0)
		{
			accountName = name;
			return;
		}
		domain = name.Substring(0, num).Trim();
		accountName = name.Substring(num + 1);
	}

	public static void AddDirectorySecurity(string directoryName, string account, FileSystemRights rights, AccessControlType controlType)
	{
		SecurityIdentifier account2 = (SecurityIdentifier)new NTAccount(account).Translate(typeof(SecurityIdentifier));
		AddDirectorySecurity(directoryName, account2, rights, controlType);
	}

	public static void AddDirectorySecurity(string directoryName, IdentityReference account, FileSystemRights rights, AccessControlType controlType)
	{
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		Computer.ReplaceAllDescendantPermissions(new DirectoryInfo(directoryName), new FileSystemAccessRule(account, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, controlType));
	}

	public static void RemoveShare(string shareName)
	{
		ManagementObjectCollection managementObjectCollection = new ManagementObjectSearcher("select * from win32_share").Get();
		ManagementObject[] array = new ManagementObject[managementObjectCollection.Count];
		ManagementBaseObject[] objectCollection = array;
		managementObjectCollection.CopyTo(objectCollection, 0);
		ManagementObject[] array2 = array;
		foreach (ManagementObject managementObject in array2)
		{
			if (!(managementObject["Type"].ToString() == "0"))
			{
				continue;
			}
			string text = managementObject["Name"].ToString();
			if (shareName == text)
			{
				switch (GetShareResult((uint)managementObject.InvokeMethod("delete", null, null).Properties["ReturnValue"].Value))
				{
				case ShareResult.AccessDenied:
					throw new DeleteShareAccessDeniedException(BackupRestoreResources.DeleteShareAccessDeniedException);
				default:
					throw new DeleteShareException(BackupRestoreResources.DeleteShareException);
				case ShareResult.Success:
					break;
				}
				break;
			}
		}
	}

	public static bool DeleteFolder(string fullFolderPath)
	{
		if (Directory.Exists(fullFolderPath))
		{
			Directory.Delete(fullFolderPath, recursive: true);
			int num = 0;
			while (Directory.Exists(fullFolderPath) && num < 20)
			{
				num++;
				Thread.Sleep(10);
			}
			if (num == 20)
			{
				return false;
			}
		}
		return true;
	}

	public static void DeleteFiles(string folderName)
	{
		if (!Directory.Exists(folderName))
		{
			return;
		}
		foreach (string item in Directory.EnumerateDirectories(folderName))
		{
			Directory.Delete(item, recursive: true);
		}
		foreach (string item2 in Directory.EnumerateFiles(folderName))
		{
			File.Delete(item2);
		}
	}

	public static bool IsLocalFolder(string folderPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (directoryInfo.Root.FullName.Equals(driveInfo.Name, StringComparison.OrdinalIgnoreCase))
			{
				return driveInfo.DriveType == DriveType.Fixed;
			}
		}
		return false;
	}

	public static long AvailableFreeSpace(string folderPath)
	{
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (directoryInfo.Root.FullName.Equals(driveInfo.Name))
				{
					return driveInfo.AvailableFreeSpace;
				}
			}
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (SecurityException)
		{
		}
		return -1L;
	}
}

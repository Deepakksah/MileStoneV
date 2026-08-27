using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Security;

public class SecureFolder : ISecureFolder
{
	public IList<FileSystemRights> BuiltInAdminRights = new List<FileSystemRights> { FileSystemRights.FullControl };

	public IDirectoryService DirectoryService{ get; }

	public IFileService FileService{ get; }

	public IList<FileSystemRights> UserAccountRights => new List<FileSystemRights>
	{
		FileSystemRights.DeleteSubdirectoriesAndFiles,
		FileSystemRights.Delete,
		FileSystemRights.Read,
		FileSystemRights.Write,
		FileSystemRights.Synchronize
	};

	public IList<FileSystemRights> UserAccountRightsReadOnly => new List<FileSystemRights>
	{
		FileSystemRights.Read,
		FileSystemRights.Synchronize
	};

	public SecureFolder(IFileService fileService, IDirectoryService directoryService)
	{
		FileService = fileService;
		DirectoryService = directoryService;
	}

	public SecureFolder()
		: this(new FileService(), new DirectoryService())
	{
	}

	public void SetSecurePermissions(string folderPath, string accountValue, bool userAccountReadOnly)
	{
		SetSecurePermissions(folderPath, accountValue, userAccountReadOnly, exclusive: true);
	}

	public void SetSecurePermissions(string folderPath, string accountValue, bool userAccountReadOnly, bool exclusive)
	{
		if (accountValue == null)
		{
			throw new ArgumentNullException("accountValue");
		}
		if (!DirectoryService.Exists(folderPath))
		{
			throw new DirectoryNotFoundException("folderPath");
		}
		IList<FileSystemRights> userAccountRights = (userAccountReadOnly ? UserAccountRightsReadOnly : UserAccountRights);
		new AccessControlUpdater(FileService, DirectoryService).SetDirectoryAccessControl(folderPath, accountValue, userAccountRights, BuiltInAdminRights, AccessControlUpdater.InheritanceOption.DisableInheritance, exclusive);
	}

	private void SetSecurePermissions(string folderPath)
	{
		if (!DirectoryService.Exists(folderPath))
		{
			throw new DirectoryNotFoundException("folderPath");
		}
		new AccessControlUpdater(FileService, DirectoryService).SetDirectoryAccessControl(folderPath, BuiltInAdminRights, AccessControlUpdater.InheritanceOption.DisableInheritance);
	}

	public void CreateSecureFolder(string folderPath, string accountValue)
	{
		CreateSecureFolder(folderPath, accountValue, userAccountReadOnly: false);
	}

	public void CreateSecureFolder(string folderPath)
	{
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		if (!DirectoryService.Exists(folderPath))
		{
			DirectoryService.CreateDirectory(folderPath);
		}
		SetSecurePermissions(folderPath);
	}

	public void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly)
	{
		if (string.IsNullOrWhiteSpace(accountValue))
		{
			throw new ArgumentException("accountValue");
		}
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		bool flag = !DirectoryService.Exists(folderPath);
		if (flag)
		{
			DirectoryService.CreateDirectory(folderPath);
		}
		SetSecurePermissions(folderPath, accountValue, userAccountReadOnly, flag);
	}

	public void CreateSecureFolder(string folderPath, string accountValue, bool userAccountReadOnly, bool exclusive)
	{
		if (string.IsNullOrWhiteSpace(accountValue))
		{
			throw new ArgumentException("accountValue");
		}
		if (folderPath == null)
		{
			throw new ArgumentNullException("folderPath");
		}
		if (!DirectoryService.Exists(folderPath))
		{
			DirectoryService.CreateDirectory(folderPath);
		}
		SetSecurePermissions(folderPath, accountValue, userAccountReadOnly, exclusive);
	}
}

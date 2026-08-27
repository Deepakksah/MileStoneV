using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Security;

public class AccessControlUpdater
{
	public enum InheritanceOption
	{
		NoChange,
		DisableInheritance,
		EnableInheritance
	}

	public static SecurityIdentifier NetworkServiceSid { get; } = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);

	public static SecurityIdentifier LocalServiceSid { get; } = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);

	public static SecurityIdentifier LocalSystemSid { get; } = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);

	public IFileService FileService{ get; private set; }

	public IDirectoryService DirectoryService{ get; private set; }

	public AccessControlUpdater(IFileService fileService, IDirectoryService directoryService)
	{
		FileService = fileService;
		DirectoryService = directoryService;
	}

	public AccessControlUpdater()
		: this(new FileService(), new DirectoryService())
	{
	}

	public void SetDirectoryAccessControl(string directoryPath, IList<FileSystemRights> builtInAdminRights, InheritanceOption inheritanceOption)
	{
		ApplyChanges(directoryPath, null, null, builtInAdminRights, inheritanceOption, exclusive: true);
	}

	public void SetDirectoryAccessControl(string directoryPath, string accountValue, IList<FileSystemRights> userAccountRights, IList<FileSystemRights> builtInAdminRights, InheritanceOption inheritanceOption)
	{
		SetDirectoryAccessControl(directoryPath, accountValue, userAccountRights, builtInAdminRights, inheritanceOption, exclusive: true);
	}

	public void SetDirectoryAccessControl(string directoryPath, string accountValue, IList<FileSystemRights> userAccountRights, IList<FileSystemRights> builtInAdminRights, InheritanceOption inheritanceOption, bool exclusive)
	{
		if (string.IsNullOrWhiteSpace(accountValue))
		{
			throw new ArgumentException("accountValue");
		}
		ApplyChanges(directoryPath, accountValue, userAccountRights, builtInAdminRights, inheritanceOption, exclusive);
	}

	private void ApplyChanges(string directoryPath, string accountName, IEnumerable<FileSystemRights> userAccountRights, IEnumerable<FileSystemRights> builtInAdminRights, InheritanceOption inheritanceOption, bool exclusive)
	{
		if (directoryPath == null)
		{
			throw new ArgumentNullException("directoryPath");
		}
		if (builtInAdminRights == null)
		{
			throw new ArgumentNullException("directoryPath");
		}
		try
		{
			DirectorySecurity accessControl = DirectoryService.GetAccessControl(directoryPath);
			switch (inheritanceOption)
			{
			case InheritanceOption.EnableInheritance:
				accessControl.SetAccessRuleProtection(isProtected: false, preserveInheritance: false);
				break;
			case InheritanceOption.DisableInheritance:
				accessControl.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
				break;
			}
			SecurityIdentifier securityIdentifier = null;
			if (!string.IsNullOrWhiteSpace(accountName))
			{
				if (userAccountRights == null)
				{
					throw new ArgumentNullException("userAccountRights");
				}
				securityIdentifier = (SecurityIdentifier)new NTAccount(accountName).Translate(typeof(SecurityIdentifier));
			}
			SecurityIdentifier securityIdentifier2 = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
			if (exclusive)
			{
				foreach (FileSystemAccessRule accessRule in accessControl.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(SecurityIdentifier)))
				{
					accessControl.RemoveAccessRule(accessRule);
				}
			}
			else
			{
				RemoveAllRights(accessControl, securityIdentifier);
				RemoveAllRights(accessControl, securityIdentifier2);
			}
			AddRights(userAccountRights, accessControl, securityIdentifier);
			AddRights(builtInAdminRights, accessControl, securityIdentifier2);
			DirectoryService.SetAccessControl(directoryPath, accessControl);
		}
		catch (Exception ex)
		{
			throw new FileAccessControlException(string.Format(CultureInfo.InvariantCulture, "SetDirectoryAccessControl: Failed to set access control for {0}, Exception: {1}", directoryPath, ex.Message), ex);
		}
	}

	private static void RemoveAllRights(DirectorySecurity dSecurity, SecurityIdentifier securityIdentifier)
	{
		if (securityIdentifier != null)
		{
			dSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(securityIdentifier, (FileSystemRights)0, AccessControlType.Allow));
		}
	}

	private static void AddRights(IEnumerable<FileSystemRights> accountRights, DirectorySecurity dSecurity, SecurityIdentifier securityIdentifier)
	{
		if (!(securityIdentifier != null))
		{
			return;
		}
		foreach (FileSystemRights accountRight in accountRights)
		{
			dSecurity.AddAccessRule(new FileSystemAccessRule(securityIdentifier, accountRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
		}
	}

	public void SetFileAccessControl(string filePath, bool isUserName, string accountValue)
	{
		if (string.IsNullOrEmpty(accountValue))
		{
			throw new ArgumentNullException("accountValue");
		}
		try
		{
			IFileSecurity accessControl = FileService.GetAccessControl(filePath);
			accessControl.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
			foreach (FileSystemAccessRule accessRule in accessControl.GetAccessRules(includeExplicit: true, includeInherited: false, typeof(NTAccount)))
			{
				if (!accessControl.RemoveAccessRule(accessRule))
				{
					throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, "SetFileAccessControl(): Could not remove an access rule from {0}", filePath));
				}
			}
			SecurityIdentifier securityIdentifier = null;
			securityIdentifier = (isUserName ? ((SecurityIdentifier)new NTAccount(accountValue).Translate(typeof(SecurityIdentifier))) : ((!accountValue.Equals("localsystem", StringComparison.OrdinalIgnoreCase)) ? new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null) : new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null)));
			accessControl.AddAccessRule(new FileSystemAccessRule(securityIdentifier, FileSystemRights.Read | FileSystemRights.Write, AccessControlType.Allow));
			SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
			accessControl.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
			SecurityIdentifier identity2 = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
			accessControl.AddAccessRule(new FileSystemAccessRule(identity2, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
			FileService.SetAccessControl(filePath, accessControl);
		}
		catch (Exception ex)
		{
			throw new FileAccessControlException(string.Format(CultureInfo.InvariantCulture, "SetFileAccessControl(): Failed to set access control for {0}, Exception: {1}", filePath, ex.Message), ex);
		}
	}

	public void SetDirectoryAccessControl(string directoryPath, string accountValue, IList<FileSystemRights> userAccountRights, IList<FileSystemRights> builtInAdminRights)
	{
		SetDirectoryAccessControl(directoryPath, accountValue, userAccountRights, builtInAdminRights, InheritanceOption.NoChange);
	}
}

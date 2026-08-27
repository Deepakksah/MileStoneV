using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace VideoOS.Installer.Common.Utilities;

public class AccessControlUpdater
{
	public enum InheritanceOption
	{
		NoChange,
		DisableInheritance,
		EnableInheritance
	}

	private string _directoryOrFilePath;

	public AccessControlUpdater(string filePath)
	{
		_directoryOrFilePath = filePath;
	}

	public void SetFileAccessControl(bool userAccount, string accountValue)
	{
		if (string.IsNullOrEmpty(accountValue))
		{
			throw new ArgumentNullException("accountValue");
		}
		try
		{
			FileSecurity accessControl = File.GetAccessControl(_directoryOrFilePath);
			accessControl.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
			foreach (FileSystemAccessRule accessRule in accessControl.GetAccessRules(includeExplicit: true, includeInherited: false, typeof(NTAccount)))
			{
				if (!accessControl.RemoveAccessRule(accessRule))
				{
					throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, "SetFileAccessControl(): Could not remove an access rule from {0}", _directoryOrFilePath));
				}
			}
			SecurityIdentifier securityIdentifier = null;
			securityIdentifier = (userAccount ? ((SecurityIdentifier)new NTAccount(accountValue).Translate(typeof(SecurityIdentifier))) : ((!accountValue.Equals("localsystem", StringComparison.OrdinalIgnoreCase)) ? new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null) : new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null)));
			accessControl.AddAccessRule(new FileSystemAccessRule(securityIdentifier, FileSystemRights.Read | FileSystemRights.Write, AccessControlType.Allow));
			SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
			accessControl.AddAccessRule(new FileSystemAccessRule(identity, FileSystemRights.FullControl, AccessControlType.Allow));
			SecurityIdentifier identity2 = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
			accessControl.AddAccessRule(new FileSystemAccessRule(identity2, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
			File.SetAccessControl(_directoryOrFilePath, accessControl);
		}
		catch (Exception ex)
		{
			throw new FileAccessControlException(string.Format(CultureInfo.InvariantCulture, "SetFileAccessControl(): Failed to set access control for {0}, Exception: {1}", _directoryOrFilePath, ex.Message), ex);
		}
	}

	public void SetDirectoryAccessControl(bool userAccount, string accountValue, IReadOnlyCollection<FileSystemRights> userAccountRights, IReadOnlyCollection<FileSystemRights> builtInAdminRights, InheritanceOption inheritanceOption)
	{
		if (string.IsNullOrEmpty(accountValue))
		{
			throw new ArgumentNullException("accountValue");
		}
		try
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(_directoryOrFilePath);
			DirectorySecurity dSecurity = directoryInfo.GetAccessControl();
			switch (inheritanceOption)
			{
			case InheritanceOption.EnableInheritance:
				dSecurity.SetAccessRuleProtection(isProtected: false, preserveInheritance: false);
				break;
			case InheritanceOption.DisableInheritance:
				dSecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
				break;
			}
			SecurityIdentifier securityIdentifier = null;
			NTAccount nTAccount = null;
			if (!userAccount)
			{
				if (accountValue.Equals("localsystem", StringComparison.OrdinalIgnoreCase))
				{
					securityIdentifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
				}
				else
				{
					securityIdentifier = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
				}
			}
			else
			{
				nTAccount = new NTAccount(accountValue);
				securityIdentifier = (SecurityIdentifier)nTAccount.Translate(typeof(SecurityIdentifier));
			}
			userAccountRights.ToList().ForEach(delegate(FileSystemRights userAccountRight)
			{
				dSecurity.AddAccessRule(new FileSystemAccessRule(securityIdentifier, userAccountRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
			});
			builtInAdminRights.ToList().ForEach(delegate(FileSystemRights adminRight)
			{
				SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
				dSecurity.AddAccessRule(new FileSystemAccessRule(identity, adminRight, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
			});
			directoryInfo.SetAccessControl(dSecurity);
		}
		catch (Exception ex)
		{
			throw new FileAccessControlException(string.Format(CultureInfo.InvariantCulture, "SetDirectoryAccessControl: Failed to set access control for {0}, Exception: {1}", _directoryOrFilePath, ex.Message), ex);
		}
	}

	public void SetDirectoryAccessControl(bool userAccount, string accountValue, IReadOnlyCollection<FileSystemRights> userAccountRights, IReadOnlyCollection<FileSystemRights> builtInAdminRights)
	{
		SetDirectoryAccessControl(userAccount, accountValue, userAccountRights, builtInAdminRights, InheritanceOption.NoChange);
	}
}

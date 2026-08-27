using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VideoOS.Common.Controls;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Management.Common;
using VideoOS.Utilities.Security;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class WindowsUserObjectPicker
{
	public static Identity AddWindowsUser()
	{
		Identity identity = new Identity();
		using (ObjectPickerEx objectPickerEx = new ObjectPickerEx())
		{
			objectPickerEx.AllowedTypes = DirectoryObjectTypes.Users | DirectoryObjectTypes.Groups | DirectoryObjectTypes.Computers | DirectoryObjectTypes.BuiltInGroups | DirectoryObjectTypes.WellKnownPrincipals;
			objectPickerEx.AllowedLocations = DirectoryLocations.LocalComputer | DirectoryLocations.JoinedDomain | DirectoryLocations.EnterpriseDomain | DirectoryLocations.GlobalCatalog | DirectoryLocations.ExternalDomain | DirectoryLocations.UserEntered;
			objectPickerEx.DefaultTypes = DirectoryObjectTypes.Users | DirectoryObjectTypes.Groups;
			objectPickerEx.DefaultLocations = DirectoryLocations.JoinedDomain;
			objectPickerEx.TargetComputer = "localhost";
			objectPickerEx.AllowedBuiltInPricipals = BuiltInPrincipals.NetworkService;
			objectPickerEx.MultiSelect = false;
			objectPickerEx.PathType = ADsPathType.Ldap;
			try
			{
				if (objectPickerEx.ShowDialog() != DialogResult.OK || objectPickerEx.SelectedObjects == null || objectPickerEx.SelectedObjects.Length == 0)
				{
					return null;
				}
			}
			catch (COMException)
			{
				Utils.ShowMessageAsError(Resources.ErrorMessage_GeneralErrorLabelHeadLine, Resources.UserPageInvalidIdentities, objectPickerEx as IWin32Window);
				return null;
			}
			try
			{
				using (new WaitCursor())
				{
					if (objectPickerEx.SelectedObjects.Any())
					{
						DirectoryObject directoryObject = objectPickerEx.SelectedObjects.First();
						if (directoryObject.BinarySid == null)
						{
							string message = string.Format(CultureInfo.InvariantCulture, Resources.UserPageInvalidIdentity, directoryObject.Name);
							Utils.ShowMessageAsError(Resources.ErrorMessage_GeneralErrorLabelHeadLine, message, GetPickerAsIWin32Window(objectPickerEx));
							return null;
						}
						if (directoryObject.ClassName.Equals("group", StringComparison.OrdinalIgnoreCase))
						{
							identity.Type = IdentityType.WindowsGroup;
						}
						else
						{
							identity.Type = IdentityType.WindowsUser;
						}
						AccountType accountType = AccountType.SidTypeUnknown;
						string domainName = null;
						string text = null;
						try
						{
							text = VideoOS.Utilities.Security.NativeMethods.SidToName(directoryObject.BinarySid, out domainName, out accountType);
						}
						catch (Win32Exception ex2)
						{
							string message2 = string.Format(CultureInfo.InvariantCulture, Resources.UserPageInvalidIdentity, directoryObject.Name);
							Utils.ShowMessageAsError(Resources.ErrorMessage_GeneralErrorLabelHeadLine, message2, ex2, GetPickerAsIWin32Window(objectPickerEx));
							return null;
						}
						if (accountType == AccountType.SidTypeDomain)
						{
							text = directoryObject.Name;
							domainName = "NT AUTHORITY";
						}
						else
						{
							identity.Sid = directoryObject.Sid;
						}
						identity.Domain = domainName;
						identity.AccountName = text;
					}
				}
			}
			catch (Exception)
			{
				Utils.ShowMessageAsError(Resources.ErrorMessage_GeneralErrorLabelHeadLine, Resources.UserPageInvalidIdentities, GetPickerAsIWin32Window(objectPickerEx));
				return null;
			}
		}
		return identity;
	}

	private static IWin32Window GetPickerAsIWin32Window(ObjectPickerEx picker)
	{
		return picker as IWin32Window;
	}
}

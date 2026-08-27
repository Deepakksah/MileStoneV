using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class RolePlugin : ProgressPlugin
{
	public RolePlugin(int serverPort)
		: base(serverPort)
	{
	}

	public ConfigurationItem[] GetRoles(ConfigurationApiServiceManager serviceManager)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		OnProgressChanged(0, Resources.ProgressGettingRoles);
		ConfigurationItem[] result = null;
		try
		{
			result = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/RoleFolder"), null);
			OnProgressChanged(100, Resources.ProgressFinishedGettingRoles);
		}
		catch (Exception exception)
		{
			OnProgressChanged(100, Resources.ProgressFailedGettingRoles);
			LogManager.Write(exception);
		}
		return result;
	}

	public bool CreateBasicUser(ConfigurationApiServiceManager serviceManager, string userName, string password)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		using (new WaitCursor())
		{
			try
			{
				ConfigurationItem basicUserFolder = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem("/BasicUserFolder"), null);
				ConfigurationItem invokeItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(basicUserFolder, "AddBasicUser"), null);
				invokeItem.Properties.SetValue("Name", userName);
				invokeItem.Properties.SetValue("password", password);
				ConfigurationItem configurationItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(invokeItem, "AddBasicUser"), null);
				string value = null;
				if (!configurationItem.Properties.TryGetPropertyValue("State", out value) || value != "Success")
				{
					return false;
				}
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
				return false;
			}
		}
		return true;
	}

	public string GetBasicUserSid(ConfigurationApiServiceManager serviceManager, string userName)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		string result = null;
		using (new WaitCursor())
		{
			try
			{
				result = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/BasicUserFolder"), null).Single((ConfigurationItem u) => u.Properties.Single((Property p) => p.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)).Value.Equals(userName, StringComparison.OrdinalIgnoreCase)).Properties.Single((Property p) => p.Key.Equals("Sid")).Value;
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
		return result;
	}

	public bool AddUserToRole(ConfigurationApiServiceManager serviceManager, string sid, string roleName)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		using (new WaitCursor())
		{
			try
			{
				ConfigurationItem[] roles = GetRoles(serviceManager);
				ConfigurationItem roleItem = roles.Single((ConfigurationItem r) => r.Properties.Single((Property p) => p.Key.Equals("Name", StringComparison.OrdinalIgnoreCase)).Value.Equals(roleName, StringComparison.OrdinalIgnoreCase));
				ConfigurationItem userFolder = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(roleItem.Path), null).Single((ConfigurationItem c) => c.ItemType == "UserFolder");
				ConfigurationItem invokeItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(userFolder, "AddRoleMember"), null);
				invokeItem.Properties.SetValue("Sid", sid);
				ConfigurationItem configurationItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(invokeItem, "AddRoleMember"), null);
				string value = null;
				if (!configurationItem.Properties.TryGetPropertyValue("State", out value) || value != "Success")
				{
					return false;
				}
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
				return false;
			}
		}
		return true;
	}

	public bool DoCreateOperatorRole(ConfigurationApiServiceManager serviceManager)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		try
		{
			OnProgressChanged(0, Resources.ProgressCreatingOperatorRole);
			ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/RoleFolder"), null);
			if (array.Length != 1)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Create Operators role failed, expected number of roles already existing to be 1, but it was {0}", array.Length), TraceEventType.Information);
				return false;
			}
			if (array.Any((ConfigurationItem f) => f.Properties.Single((Property p) => p.Key == "Name").Value == "Operators"))
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Create Operators role failed, the Operators Role already exists"), TraceEventType.Information);
				return false;
			}
			ConfigurationItem roleFolder = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem("/RoleFolder"), null);
			ConfigurationItem invokeItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(roleFolder, "AddRole"), null);
			invokeItem.Properties.SetValue("Name", "Operators");
			invokeItem.Properties.SetValue("Description", "Default role created by the installer");
			invokeItem.Properties.SetValue("DualAuthorizationRequired", value: false);
			invokeItem.Properties.SetValue("MakeUsersAnonymousDuringPTZSession", value: false);
			ConfigurationItem invokeResultItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(invokeItem, "AddRole"), null);
			string value = null;
			if (!invokeResultItem.Properties.TryGetPropertyValue("State", out value) || value != "Success")
			{
				if (invokeResultItem.Properties.TryGetPropertyValue("ErrorText", out var value2) && !string.IsNullOrEmpty(value2))
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Create Operator role failed with error: {0}", value2), TraceEventType.Information);
				}
				else
				{
					LogManager.Write("Create Operator role failed, but could not get error message.", TraceEventType.Information);
				}
				return false;
			}
			OnProgressChanged(50, Resources.ProgressOperatorRoleCreated);
			ConfigurationItem roleItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(invokeResultItem.Path), null);
			OnProgressChanged(75, Resources.ProgressChangingSecurityOnRole);
			ChangeSecurityOnRole(serviceManager, roleItem);
			OnProgressChanged(100, Resources.ProgressSecurityOnRoleChanged);
			return true;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			OnProgressChanged(100, Resources.ProgressCreatingOperatorRoleFailed);
		}
		return false;
	}

	private static void ChangeSecurityOnRole(ConfigurationApiServiceManager serviceManager, ConfigurationItem roleItem)
	{
		ChangeSecurityOnRole(serviceManager, roleItem, "Cameras");
		ChangeSecurityOnRole(serviceManager, roleItem, "Microphones");
		ChangeSecurityOnRole(serviceManager, roleItem, "Speakers");
		ChangeSecurityOnRole(serviceManager, roleItem, "View Groups");
	}

	private static void ChangeSecurityOnRole(ConfigurationApiServiceManager serviceManager, ConfigurationItem roleItem, string item)
	{
		ConfigurationItem namespaceSelectionInvokeInfo = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(roleItem, "ChangeOverallSecurityPermissions"), null);
		Property property = namespaceSelectionInvokeInfo.Properties.FirstOrDefault((Property p) => p.Key.Equals("SecurityNamespace", StringComparison.Ordinal));
		ValueTypeInfo valueTypeInfo = property.ValueTypeInfos.FirstOrDefault((ValueTypeInfo info) => info.Name.Equals(item));
		property.Value = valueTypeInfo.Value;
		ConfigurationItem changeSecurityPermissionInvokeInfo = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(namespaceSelectionInvokeInfo, "ChangeOverallSecurityPermissions"), null);
		changeSecurityPermissionInvokeInfo.Properties.SetValue("GENERIC_READ", "Allow");
		if (item.Equals("Cameras", StringComparison.OrdinalIgnoreCase))
		{
			changeSecurityPermissionInvokeInfo.Properties.SetValue("VIEW_LIVE", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("SMARTSEARCH", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("PLAYBACK", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_SEQUENCES", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("CREATE_BOOKMARKS", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_BOOKMARKS", "Allow");
		}
		else if (item.Equals("Microphones", StringComparison.OrdinalIgnoreCase))
		{
			changeSecurityPermissionInvokeInfo.Properties.SetValue("LISTEN", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("PLAYBACK", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_SEQUENCES", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("CREATE_BOOKMARKS", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_BOOKMARKS", "Allow");
		}
		else if (item.Equals("Speakers", StringComparison.OrdinalIgnoreCase))
		{
			changeSecurityPermissionInvokeInfo.Properties.SetValue("LISTEN", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("SPEAK", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("PLAYBACK", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_SEQUENCES", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("CREATE_BOOKMARKS", "Allow");
			changeSecurityPermissionInvokeInfo.Properties.SetValue("READ_BOOKMARKS", "Allow");
		}
		serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(changeSecurityPermissionInvokeInfo, "ChangeOverallSecurityPermissions"), null);
	}
}

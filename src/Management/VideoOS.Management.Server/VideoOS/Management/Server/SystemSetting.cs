using System;
using VideoOS.Management.Server.Log;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public static class SystemSetting
{
	public static string GetSystemSetting(IGeminiRequestContext requestContext, string name)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (!SystemSettingsManager.HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		SystemSettingsManager.CheckGetPermissions(name, requestContext);
		string value = null;
		using (IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent())
		{
			value = managementServerComponent.GetSystemSetting(name);
		}
		return SystemSettingsManager.ConvertToClientValue(requestContext, name, value);
	}

	public static void SetSystemSetting(IGeminiRequestContext requestContext, string name, string value)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (!SystemSettingsManager.HasSetting(name))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		if (!Enum.TryParse<SettingList>(name, out var result))
		{
			throw new SystemSettingNotSupportedException(name);
		}
		SystemSettingsManager.CheckSetPermissions(name, requestContext);
		Validation.CheckStringNotNullOrWhiteSpace(name, "Name");
		Validation.EnsureStringNotNull(ref value, "Value");
		string value2 = SystemSettingsManager.ConvertToDatabaseValue(requestContext, name, value);
		using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
		if (!managementServerComponent.SetSystemSetting(name, value2))
		{
			return;
		}
		Integration.TryNotifyServerAsync();
		switch (result)
		{
		case SettingList.ActivationCredentials:
		{
			using VmoLogManager vmoLogManager2 = new VmoLogManager(requestContext);
			vmoLogManager2.LogEntryAuditLogger.AuditAutomaticActivationCredentialsChanged(requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			break;
		}
		case SettingList.EnableAutomaticActivation:
		{
			using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
			if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
			{
				vmoLogManager.LogEntryAuditLogger.AuditAutomaticActivationEnabled(requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			else
			{
				vmoLogManager.LogEntryAuditLogger.AuditAutomaticActivationDisabled(requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			break;
		}
		}
	}
}

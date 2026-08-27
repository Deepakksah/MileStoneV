using System;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public class EnableAutomaticActivationManager : ISystemSettingManager
{
	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("Value cannot be an empty string.", "value");
		}
		if (!bool.TryParse(value, out var result))
		{
			throw new FormatException("The value is not a boolean!");
		}
		if (result)
		{
			using IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent();
			if (managementServerComponent.GetSystemSetting(SettingList.ActivationCredentials) == null)
			{
				throw new AutomaticActivationUserCredentialsMissingException();
			}
		}
		return value;
	}

	public string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public void CheckGetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
	}

	public void CheckSetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
	}
}

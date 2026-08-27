using System;

namespace VideoOS.Management.Server;

public class GenericUrlManager : ISystemSettingManager
{
	public string SettingName { get; private set; }

	public bool AllowBlank { get; private set; }

	public GenericUrlManager(string settingName, bool allowBlank)
	{
		SettingName = settingName;
		AllowBlank = allowBlank;
	}

	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (string.IsNullOrEmpty(value) && AllowBlank)
		{
			return string.Empty;
		}
		if (Uri.TryCreate(value, UriKind.Absolute, out var result) && !result.IsFile)
		{
			return value;
		}
		throw new SystemSettingInvalidValueException(SettingName, value);
	}

	public string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
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

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}
}

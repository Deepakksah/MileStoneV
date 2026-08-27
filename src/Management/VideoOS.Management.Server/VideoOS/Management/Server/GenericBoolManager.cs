namespace VideoOS.Management.Server;

public class GenericBoolManager : ISystemSettingManager
{
	public string SettingName { get; private set; }

	public GenericBoolManager(string settingName)
	{
		SettingName = settingName;
	}

	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		if (!bool.TryParse(value, out var _))
		{
			throw new SystemSettingInvalidValueException(SettingName, value);
		}
		return value;
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

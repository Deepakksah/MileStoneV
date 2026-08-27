namespace VideoOS.Management.Server;

public class GenericMinMaxIntegerManager : ISystemSettingManager
{
	public int MinValue { get; private set; }

	public int MaxValue { get; private set; }

	public string SettingName { get; private set; }

	public GenericMinMaxIntegerManager(string settingName, int minValue, int maxValue)
	{
		SettingName = settingName;
		MinValue = minValue;
		MaxValue = maxValue;
	}

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		int result = 0;
		if (!int.TryParse(value, out result))
		{
			throw new SystemSettingInvalidValueException(SettingName, value);
		}
		if (result < MinValue || result > MaxValue)
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
}

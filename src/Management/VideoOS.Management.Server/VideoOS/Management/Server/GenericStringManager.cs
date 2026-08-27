namespace VideoOS.Management.Server;

public class GenericStringManager : ISystemSettingManager
{
	public string SettingName { get; private set; }

	public GenericStringManager(string settingName)
	{
		SettingName = settingName;
	}

	public virtual string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public virtual string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}

	public virtual void CheckGetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
	}

	public virtual void CheckSetPermission(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
	}

	public string ConvertToClientValue(IGeminiRequestContext requestContext, string value)
	{
		return value;
	}
}

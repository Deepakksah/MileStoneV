namespace VideoOS.Management.Server;

public interface ISystemSettingManager
{
	string ConvertToDatabaseValue(IGeminiRequestContext requestContext, string value);

	string ConvertToInternalServerValue(IGeminiRequestContext requestContext, string value);

	string ConvertToClientValue(IGeminiRequestContext requestContext, string value);

	void CheckGetPermission(IGeminiRequestContext requestContext);

	void CheckSetPermission(IGeminiRequestContext requestContext);
}

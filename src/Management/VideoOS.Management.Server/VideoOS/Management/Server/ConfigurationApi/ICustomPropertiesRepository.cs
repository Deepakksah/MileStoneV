namespace VideoOS.Management.Server.ConfigurationApi;

public interface ICustomPropertiesRepository
{
	CustomProperty[] GetCustomProperties(IGeminiRequestContext requestContext, string objectId);

	void UpdateCustomProperties(IGeminiRequestContext requestContext, string objectId, CustomProperty[] customPropertyArray);

	void DeleteCustomProperty(IGeminiRequestContext requestContext, string objectId, string name);

	bool DeviceExists(IGeminiRequestContext requestContext, string deviceId);

	string GetDeviceType(IGeminiRequestContext requestContext, string deviceId);
}

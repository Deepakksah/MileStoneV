using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class DeleteAllRegistrationNumbers : IMethod
{
	private readonly ILPRMatchListClient _client;

	public string Id => "MethodIdDeleteAllRegistrationNumbers";

	public DeleteAllRegistrationNumbers(ILPRMatchListClient client)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\DeleteAllRegistrationNumbers.cs", 19);
		_client = client;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		return InvokeMethod(MethodTypesHelper.CreateInvokeInfo(Id, ConfigurationApiResources.MethodDeleteAllRegistrationNumbers, item), path);
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item, ConfigurationItemPath path)
	{
		_client.DeleteAllLicensePlates(new Guid(path.Id));
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}
}

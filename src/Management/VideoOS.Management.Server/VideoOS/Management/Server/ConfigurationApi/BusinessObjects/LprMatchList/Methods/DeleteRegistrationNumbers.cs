using System;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class DeleteRegistrationNumbers : IMethod
{
	private readonly ILPRMatchListClient _client;

	private readonly ILicensePlateItemsBuilder _builder;

	public string Id => "MethodIdDeleteRegistrationNumbers";

	public DeleteRegistrationNumbers(ILPRMatchListClient client, ILicensePlateItemsBuilder builder)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\DeleteRegistrationNumbers.cs", 26);
		builder.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\DeleteRegistrationNumbers.cs", 27);
		_client = client;
		_builder = builder;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(item, path);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodDeleteRegistrationNumbers, item, CreateRegistrationNumbersProperty());
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item, ConfigurationItemPath path)
	{
		Property property = item.Properties?.FirstOrDefault((Property s) => s.Key == "RegistrationNumbers");
		Validation.CheckStringNotNullOrWhiteSpace(property?.Value, "PropertyNameRegistrationNumbers");
		LicensePlate[] licensePlates = _builder.BuildLicensePlates(property.Value);
		_client.DeleteLicensePlates(new Guid(path.Id), licensePlates);
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static Property CreateRegistrationNumbersProperty()
	{
		return GeneralPropertyDefinitions.GetListProperty(string.Empty, "RegistrationNumbers", ConfigurationApiResources.PropertyMatchListRegistrationNumbers, "PropertyMatchListRegistrationNumbers", isSettable: true);
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Builders;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class AddMatchList : IMethod
{
	private readonly ILPRMatchListClient _client;

	private readonly IConfigurationItemBuilder _itemBuilder;

	public string Id => "MethodIdAddLprMatchList";

	public AddMatchList(ILPRMatchListClient client, IConfigurationItemBuilder itemBuilder)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\AddMatchList.cs", 23);
		itemBuilder.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\AddMatchList.cs", 24);
		_client = client;
		_itemBuilder = itemBuilder;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(item);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodAddLprMatchList, item, CreateNameProperty());
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value) || string.IsNullOrEmpty(value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or value not specified: {0}", "Name"));
		}
		Guid listId = Guid.NewGuid();
		try
		{
			_client.AddLicensePlateList(listId, value);
		}
		catch (Exception innerException)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Adding license plate list failed as another list with the same name already exists."), innerException);
		}
		LicensePlateList licensePlateList = _client.GetLicensePlateList(listId);
		if (licensePlateList == null)
		{
			throw new PropertyInvalidException("Adding license plate list failed");
		}
		ConfigurationItem configurationItem = _itemBuilder.Build(ConfigurationItemPath.CreateItemPath("LprMatchList", listId.ToString()), ConfigurationItemPath.CreatePath(item.Path), licensePlateList, new LicensePlateInfo[0], new string[0]);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static Property CreateNameProperty()
	{
		return GeneralPropertyDefinitions.GetNameProperty("", isSettable: true);
	}
}

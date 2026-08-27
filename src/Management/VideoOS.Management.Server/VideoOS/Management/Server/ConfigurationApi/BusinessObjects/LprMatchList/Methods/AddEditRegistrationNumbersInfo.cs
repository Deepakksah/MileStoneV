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

public class AddEditRegistrationNumbersInfo : IMethod
{
	private readonly ILPRMatchListClient _client;

	private readonly ILicensePlateItemsBuilder _builder;

	public string Id => "MethodIdAddOrEditRegistrationNumbersInfo";

	public AddEditRegistrationNumbersInfo(ILPRMatchListClient client, ILicensePlateItemsBuilder builder)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\AddEditRegistrationNumbersInfo.cs", 26);
		builder.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\AddEditRegistrationNumbersInfo.cs", 27);
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
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodAddEditRegistrationNumbersInfo, item, CreateRegistrationNumbersProperty());
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegistrationNumbers", out string value) || string.IsNullOrEmpty(value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or value not specified: {0}", "RegistrationNumbers"));
		}
		LicensePlateInfo[] licensePlates = _builder.BuildLicensePlateInfos(value);
		_client.AddLicensePlates(new Guid(path.Id), licensePlates);
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
	}

	private static Property CreateRegistrationNumbersProperty()
	{
		return GeneralPropertyDefinitions.GetListProperty(string.Empty, "RegistrationNumbers", ConfigurationApiResources.PropertyMatchListRegistrationNumbersDataList, "PropertyMatchListRegistrationNumbersDataList", isSettable: true);
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.Converters;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class GetCustomFieldsForRegistrationNumber : IMethod
{
	private readonly ILPRMatchListClient _client;

	private readonly IStringCollectionConverter _stringCollectionConverter;

	public string Id => "MethodIdGetCustomFieldsForRegistrationNumber";

	public GetCustomFieldsForRegistrationNumber(ILPRMatchListClient client, IStringCollectionConverter stringCollectionConverter)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\GetCustomFieldsForRegistrationNumber.cs", 26);
		_client = client;
		_stringCollectionConverter = stringCollectionConverter;
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
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodGetCustomFieldsForRegistrationNumber, item, CreateRegistrationNumberProperty());
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RegistrationNumber", out string number) || string.IsNullOrEmpty(number))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or value not specified: {0}", "RegistrationNumber"));
		}
		LicensePlateInfo licensePlateInfo = _client.GetLicensePlates(new Guid(path.Id), number).FirstOrDefault((LicensePlateInfo s) => s.LicensePlate.RegistrationNumber == number);
		if (licensePlateInfo == null)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "License plate cannot be found: {0}", number));
		}
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true, CreateCustomFieldsProperty(from s in licensePlateInfo.Properties
			orderby s.ColumnIndex
			select s.Value));
	}

	private static Property CreateRegistrationNumberProperty()
	{
		return GeneralPropertyDefinitions.GetStringProperty("RegistrationNumber", "", ConfigurationApiResources.PropertyMatchListRegistrationNumber, "PropertyMatchListRegistrationNumber", isSettable: true);
	}

	private Property CreateCustomFieldsProperty(IEnumerable<string> values)
	{
		return GeneralPropertyDefinitions.GetListProperty(_stringCollectionConverter.EnumerableToString(values), "CustomFields", ConfigurationApiResources.PropertyMatchListCustomFieldValuesList, "PropertyMatchListCustomFieldValuesList", isSettable: false);
	}
}

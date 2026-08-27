using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Management.Server.ConfigurationApi.LprClient;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.BusinessObjects.LprMatchList.Methods;

public class RemoveMatchList : IMethod
{
	private readonly ILPRMatchListClient _client;

	public string Id => "MethodIdRemoveLprMatchList";

	public RemoveMatchList(ILPRMatchListClient client)
	{
		client.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\LPRMatchList\\Methods\\RemoveMatchList.cs", 23);
		_client = client;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(item);
	}

	private ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		IDictionary<string, string> listMembers = GetListMembers();
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodRemoveLprMatchList, item, CreateIdProperty(listMembers));
	}

	private ConfigurationItem InvokeMethod(ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out string value) || string.IsNullOrEmpty(value) || !Guid.TryParse(value, out var result) || result == Guid.Empty)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or the value not specified correctly: {0}", "Id"));
		}
		_client.DeleteLicensePlateList(result);
		return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Success", false);
	}

	private IDictionary<string, string> GetListMembers()
	{
		return _client.GetLicensePlateLists().ToDictionary((LicensePlateList s) => string.Format(CultureInfo.InvariantCulture, "{0}", s.Id), (LicensePlateList s) => s.Name);
	}

	private static Property CreateIdProperty(IDictionary<string, string> members)
	{
		return GeneralPropertyDefinitions.GetEnumProperty("Id", "", ConfigurationApiResources.PropertyGeneralId, "PropertyGeneralId", "", members);
	}
}

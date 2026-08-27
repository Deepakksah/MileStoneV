using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.MethodHandling;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl.Methods;

public class GetInternalUnitPath : IMethod
{
	private IMIPObjectProxy _mipObjectProxy;

	public string Id => "GetInternalUnitPath";

	public GetInternalUnitPath(IMIPObjectProxy mipObjectProxy)
	{
		_mipObjectProxy = mipObjectProxy;
	}

	public ConfigurationItem Invoke(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return CreateInvokeInfo(item, Id);
		}
		return InvokeMethod(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfo(ConfigurationItem item, string methodId)
	{
		return MethodTypesHelper.CreateInvokeInfo(methodId, ConfigurationApiResources.MethodGetInternalUnitPath, item, GeneralPropertyDefinitions.GetStringProperty("ExternalId", string.Empty, ConfigurationApiResources.PropertyAccessControlExternalId, "PropertyAccessControlExternalId", isSettable: true));
	}

	private ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ExternalId", out string value) || string.IsNullOrEmpty(value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Property not found or value not specified: {0}", "ExternalId"));
		}
		Guid guid = GuidUtil.Create(new FQID(_mipObjectProxy.QueryMIPObject(requestContext, path.Id).CustomData).ServerId.Id, value);
		string value2 = string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", "AccessControlUnit", guid.ToString());
		Property pathProperty = GeneralPropertyDefinitions.GetPathProperty("InternalUnitPath", ConfigurationApiResources.PropertyAccessControlInternalUnitPath, value2, new Collection<string> { "AccessControlUnit" }, "PropertyAccessControlInternalUnitPath", isSettable: false);
		return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true, pathProperty);
	}
}

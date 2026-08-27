using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public static class Constants
{
	public const string PropertyNameRuleGroup = "RuleGroup";

	public const string PropertyNameExternalId = "ExternalId";

	public const string PropertyNameInternalUnitPath = "InternalUnitPath";

	public const string AccessControlRuleGroupItemType = "AccessControlRuleGroup";

	public const string AccessControlSourceItemKind = "6d3bdb33-d913-470f-8784-3dd4ee94db8c";

	public static Guid AccessControlPluginId = new Guid("BB529DE8-608E-4EBB-B796-889504E54670");

	public static Guid AccessControlSecurityNamespace = SecurityNamespaceIdentifier.AccessControlUnits;

	public const string AccessControlUngroupedId = "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF";
}

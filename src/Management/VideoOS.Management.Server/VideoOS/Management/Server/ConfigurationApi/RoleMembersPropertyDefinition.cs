using System;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RoleMembersPropertyDefinition
{
	public static Property IdentityTypeProperty(IdentityType identityType)
	{
		return new Property("IdentityType", identityType.ToString())
		{
			ValueType = "Enum",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberIdentityType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberIdentityType",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = PropertyTypesHelper.GetPropertyValueInfoItems(typeof(IdentityType))
		};
	}

	public static Property AccountNameProperty(string accountName)
	{
		return new Property("AccountName", accountName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberAccountName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberAccountName",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DisplayNameProperty(string displayName)
	{
		return new Property("DisplayName", displayName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberDisplayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberDisplayName",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DomainProperty(string domain)
	{
		return new Property("Domain", domain)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberDomain,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberDomain",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DomainAccountNameProperty(string domainAccountName)
	{
		return new Property("DomainAccountName", domainAccountName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberDomainAccountName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberDomainAccountName",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property DistinguishedNameProperty(string distinguishedName)
	{
		return new Property("DomainAccountName", distinguishedName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberDistinguishedName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberDistinguishedName",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MemberOfGroupsProperty(string[] memberOfGroups)
	{
		return new Property("MemberOf", string.Join(",", memberOfGroups))
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberMemberOfGroups,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberMemberOfGroups",
			ToolTipTranslationId = "PropertyRoleMemberMemberOfGroupsToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MemberOfRolesProperty(Guid[] memberOfRoles)
	{
		return new Property("MemberOfRoles", string.Join(",", memberOfRoles))
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberMemberOfRoles,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberMemberOfRoles",
			ToolTipTranslationId = "PropertyRoleMemberMemberOfRolesToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MembersProperty(string[] members)
	{
		return new Property("Members", string.Join(",", members))
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRoleMemberMembers,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRoleMemberMembers",
			ToolTipTranslationId = "PropertyRoleMemberMembersToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}
}

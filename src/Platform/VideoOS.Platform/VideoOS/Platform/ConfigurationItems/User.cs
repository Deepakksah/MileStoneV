using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class User : IConfigurationItem
{
	public string Sid => base.Wrapper.GetPropertyValue("Sid");

	public Dictionary<string, string> IdentityTypeValues => base.Wrapper.GetEnumValues("IdentityType");

	public string IdentityType => base.Wrapper.GetPropertyValue("IdentityType");

	public string AccountName => base.Wrapper.GetPropertyValue("AccountName");

	public override string DisplayName => base.Wrapper.GetPropertyValue("DisplayName");

	public string Domain => base.Wrapper.GetPropertyValue("Domain");

	public string MemberOf => base.Wrapper.GetPropertyValue("MemberOf");

	public string MemberOfRoles => base.Wrapper.GetPropertyValue("MemberOfRoles");

	public string Members => base.Wrapper.GetPropertyValue("Members");

	public User(ServerId serverId, ConfigurationItem user)
		: base(serverId, user)
	{
	}

	public User(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public User(FQID fqid)
		: base(fqid)
	{
	}
}

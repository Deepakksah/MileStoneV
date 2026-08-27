using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Helper;

public static class FilterParameters
{
	public static readonly IReadOnlyDictionary<Type, HashSet<string>> WhitelistedSearchTermsByType = new Dictionary<Type, HashSet<string>> { 
	{
		typeof(VmsUser),
		new HashSet<string>
		{
			"Id", "UserName", "Email", "CanChangePassword", "CreatedDate", "Description", "EmailConfirmed", "LockoutEnd", "Active", "ModifiedDate",
			"PhoneNumber"
		}
	} }.ToImmutableDictionary();
}

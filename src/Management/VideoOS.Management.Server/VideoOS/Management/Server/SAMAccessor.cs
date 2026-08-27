using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class SAMAccessor : IdentityAccessor
{
	public SAMAccessor(string sid, string domain, string userName, AccountType accountType)
		: base(sid, domain, userName, accountType)
	{
	}

	public override Identity QueryIdentity(IGeminiRequestContext requestContext, bool includeMembers)
	{
		Identity identity = new Identity();
		using (DirectoryEntry directoryEntry = new DirectoryEntry(IdentityAccessor._WinNTToken + "./" + _userName))
		{
			string schemaClassName = directoryEntry.SchemaClassName;
			if (schemaClassName.Equals("user", StringComparison.OrdinalIgnoreCase))
			{
				identity.Type = IdentityType.WindowsUser;
			}
			else if (schemaClassName.Equals("group", StringComparison.OrdinalIgnoreCase))
			{
				identity.Type = IdentityType.WindowsGroup;
			}
			else
			{
				identity.Type = IdentityType.UnknownIdentityType;
			}
			identity.AccountName = (string)directoryEntry.Properties["name"].Value;
			if (string.IsNullOrEmpty(identity.AccountName))
			{
				identity.AccountName = directoryEntry.Name;
			}
			identity.Sid = IdentityAccessor.GetSidFromBinarySid(directoryEntry.Properties["objectSid"].Value);
			if (string.IsNullOrEmpty(identity.Sid) || string.IsNullOrEmpty(identity.AccountName))
			{
				return null;
			}
			if (directoryEntry.Properties.Contains("fullName"))
			{
				identity.DisplayName = IdentityAccessor.NormalizeString((string)directoryEntry.Properties["fullName"].Value);
			}
			if (directoryEntry.Properties.Contains("description"))
			{
				identity.Description = IdentityAccessor.NormalizeString((string)directoryEntry.Properties["description"].Value);
			}
			identity.DistinguishedName = IdentityAccessor._LocalToken + identity.AccountName;
			identity.Domain = _domain;
			if (includeMembers && identity.Type == IdentityType.WindowsGroup)
			{
				List<string> list = new List<string>();
				foreach (object item in directoryEntry.Invoke("members", null) as IEnumerable)
				{
					using DirectoryEntry directoryEntry2 = new DirectoryEntry(item);
					if (directoryEntry2.Properties.Contains("objectSid"))
					{
						string sidFromBinarySid = IdentityAccessor.GetSidFromBinarySid(directoryEntry2.Properties["objectSid"].Value);
						if (!string.IsNullOrEmpty(sidFromBinarySid))
						{
							list.Add(sidFromBinarySid);
						}
					}
				}
				identity.Members = new string[list.Count];
				list.CopyTo(identity.Members);
			}
			if (string.IsNullOrEmpty(identity.DisplayName))
			{
				identity.DisplayName = identity.AccountName;
			}
		}
		return identity;
	}
}

using System;
using System.Globalization;
using System.Security.Principal;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public static class SecurityValidation
{
	public static void CheckObjectId(ref string objectId, string parameterName, bool allowNull)
	{
		if (!(objectId == null && allowNull))
		{
			if (objectId == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			objectId = objectId.Trim().ToUpperInvariant();
			if (objectId.Length == 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62010, objectId, parameterName), parameterName);
			}
		}
	}

	public static void CheckObjectIdArray(string[] objectIdArray, string parameterName)
	{
		if (objectIdArray == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (objectIdArray.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62021, parameterName), parameterName);
		}
		for (int i = 0; i < objectIdArray.Length; i++)
		{
			CheckObjectId(ref objectIdArray[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i), allowNull: false);
		}
	}

	public static void CheckObjectClassId(ref string objectClassId, string parameterName)
	{
		if (objectClassId == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		objectClassId = objectClassId.Trim();
		if (objectClassId.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62012, objectClassId, parameterName), parameterName);
		}
	}

	public static void CheckParentObjectNotSelf(string objectId, string parentObjectId, string parameterName)
	{
		if (objectId.Equals(parentObjectId, StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62023, parameterName, objectId), parameterName);
		}
	}

	public static void CheckActionId(ref string actionId, string parameterName)
	{
		if (actionId == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		actionId = actionId.Trim();
		if (actionId.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62011, actionId, parameterName), parameterName);
		}
	}

	public static void CheckActionIdArray(string[] actionIdArray, string parameterName)
	{
		if (actionIdArray == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (actionIdArray.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62020, parameterName), parameterName);
		}
		for (int i = 0; i < actionIdArray.Length; i++)
		{
			CheckActionId(ref actionIdArray[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i));
		}
	}

	public static void CheckAccessControlEntry(AccessControlEntry entry, string parameterName)
	{
		if (entry == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		string actionId = entry.ActionId;
		CheckActionId(ref actionId, parameterName + ".ActionId");
		entry.ActionId = actionId;
		string memberSid = entry.Sid;
		CheckSid(ref memberSid, parameterName + ".Sid");
		entry.Sid = memberSid;
	}

	public static void CheckAccessControlTimeProfileEntry(AccessControlTimeProfileEntry entry, string parameterName)
	{
		if (entry == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		CheckActionId(ref entry._actionId, parameterName + ".ActionId");
		CheckSid(ref entry._sid, parameterName + ".Sid");
	}

	public static void CheckAccessControlEntryArray(AccessControlEntry[] entries, string parameterName)
	{
		if (entries == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		for (int i = 0; i < entries.Length; i++)
		{
			CheckAccessControlEntry(entries[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i));
		}
	}

	public static void CheckAccessControlTimeProfileEntryArray(AccessControlTimeProfileEntry[] entries, string parameterName)
	{
		if (entries == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		for (int i = 0; i < entries.Length; i++)
		{
			CheckAccessControlTimeProfileEntry(entries[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i));
		}
	}

	public static void CheckSid(ref string memberSid, string parameterName)
	{
		if (memberSid == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		memberSid = memberSid.Trim();
		if (memberSid.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62013, memberSid, parameterName), parameterName);
		}
		if (!memberSid.IsGuid())
		{
			new SecurityIdentifier(memberSid);
		}
	}

	public static void CheckSidArray(string[] identityArray, string parameterName)
	{
		if (identityArray == null)
		{
			throw new ArgumentNullException(parameterName);
		}
		if (identityArray.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62019, parameterName), parameterName);
		}
		for (int i = 0; i < identityArray.Length; i++)
		{
			CheckSid(ref identityArray[i], string.Format(CultureInfo.CurrentCulture, "{0}[{1}]", parameterName, i));
		}
	}

	public static void CheckFactorAndValue(SearchFactor factor, ref string factorValue, string factorParameterName, string valueParameterName)
	{
		switch (factor)
		{
		case SearchFactor.Sid:
			CheckSid(ref factorValue, valueParameterName);
			break;
		case SearchFactor.AccountName:
			CheckAccountName(ref factorValue, valueParameterName);
			break;
		case SearchFactor.DistinguishedName:
			CheckDistinguishedName(ref factorValue, valueParameterName);
			break;
		default:
			throw new ArgumentException(VmoResources.VMO62001, factorParameterName);
		}
	}

	public static void CheckFactorAndValueArray(SearchFactor factor, ref string[] factorValueArray, string factorParameterName, string valueParameterName)
	{
		for (int i = 0; i < factorValueArray.Length; i++)
		{
			CheckFactorAndValue(factor, ref factorValueArray[i], factorParameterName, valueParameterName);
		}
	}

	public static void CheckQueryMembership(QueryMembership queryMembership, string parameterName)
	{
		if (queryMembership != QueryMembership.None && queryMembership != QueryMembership.Direct && queryMembership != QueryMembership.Expanded)
		{
			throw new ArgumentException(VmoResources.VMO62001, parameterName);
		}
	}

	public static void CheckAccountName(ref string accountName, string parameterName)
	{
		if (accountName == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		accountName = accountName.Trim();
		if (accountName.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62014, accountName), parameterName);
		}
	}

	public static void CheckDistinguishedName(ref string distinguishedName, string parameterName)
	{
		if (distinguishedName == null)
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		distinguishedName = distinguishedName.Trim();
		if (distinguishedName.Length == 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62015, distinguishedName), parameterName);
		}
	}

	public static void CheckRoleName(ref string roleName, string parameterName)
	{
		if (string.IsNullOrEmpty(roleName))
		{
			throw new ArgumentNullException(parameterName, VmoResources.VMO62002);
		}
		if (roleName.Length > 256 || roleName.Equals("Administrators"))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61006, roleName), parameterName);
		}
	}
}

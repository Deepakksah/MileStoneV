using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class UrlReservationInfoCollectionWrapper : IUrlReservationInfoCollection, IEnumerable<UrlReservationAccessControlEntry>, IEnumerable
{
	public UrlPrefix UrlReservation { get; }

	public int Count => Access.Length;

	public UrlReservationAccessControlEntry this[int index] => Access[index];

	private UrlReservationAccessControlEntry[] Access { get; }

	public UrlReservationInfoCollectionWrapper(UrlAclInfo urlAclInfo)
	{
		if (urlAclInfo == null)
		{
			throw new ArgumentNullException("urlAclInfo");
		}
		if (string.IsNullOrEmpty(urlAclInfo.Permissions))
		{
			throw new ArgumentException("Missing permissions on the URL ACL object.", "urlAclInfo");
		}
		UrlReservation = UrlPrefix.Parse(urlAclInfo.UrlPrefix);
		RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(urlAclInfo.Permissions);
		List<UrlReservationAccessControlEntry> list = new List<UrlReservationAccessControlEntry>(rawSecurityDescriptor.DiscretionaryAcl.Count);
		foreach (KnownAce item in rawSecurityDescriptor.DiscretionaryAcl.Cast<KnownAce>())
		{
			SecurityIdentifier securityIdentifier = item.SecurityIdentifier;
			AccessMask permissionFlags = ((item.AceType == AceType.AccessAllowed) ? ((AccessMask)item.AccessMask) : AccessMask.None);
			list.Add(new UrlReservationAccessControlEntry(securityIdentifier, permissionFlags));
		}
		Access = list.ToArray();
	}

	public IEnumerator<UrlReservationAccessControlEntry> GetEnumerator()
	{
		UrlReservationAccessControlEntry[] access = Access;
		for (int i = 0; i < access.Length; i++)
		{
			yield return access[i];
		}
	}

	public static UrlAclInfo ToUrlAclInfo(UrlPrefix prefix, UrlReservationAccessControlEntry[] accessControlEntries)
	{
		RawAcl rawAcl = new RawAcl(1, accessControlEntries.Length);
		byte[] opaque = new byte[0];
		for (int i = 0; i < accessControlEntries.Length; i++)
		{
			rawAcl.InsertAce(i, new CommonAce(AceFlags.None, AceQualifier.AccessAllowed, (int)accessControlEntries[i].PermissionFlags, accessControlEntries[i].Identity, isCallback: false, opaque));
		}
		RawSecurityDescriptor rawSecurityDescriptor = new RawSecurityDescriptor(ControlFlags.DiscretionaryAclPresent, null, null, null, rawAcl);
		return new UrlAclInfo(prefix.ToString(), rawSecurityDescriptor.GetSddlForm(AccessControlSections.Access));
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}

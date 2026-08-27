using System;

namespace VideoOS.Common.Integration.Security;

public class LegacySecurityActionInformation
{
	public string LegacySecurityAttribute { get; private set; }

	public string LegacySecurityArea { get; private set; }

	public Guid NamespaceId { get; private set; }

	public string SecurityAction { get; private set; }

	public string SecurityActionKey => GenerateSecurityActionKey(NamespaceId, SecurityAction);

	public LegacySecurityActionInformation(string legacySecurityAttribute, string legacySecurityArea, Guid namespaceId, string securityAction)
	{
		LegacySecurityAttribute = legacySecurityAttribute;
		LegacySecurityArea = legacySecurityArea;
		NamespaceId = namespaceId;
		SecurityAction = securityAction;
	}

	public static string GenerateSecurityActionKey(Guid namespaceId, string securityAction)
	{
		return namespaceId.ToString() + securityAction;
	}
}

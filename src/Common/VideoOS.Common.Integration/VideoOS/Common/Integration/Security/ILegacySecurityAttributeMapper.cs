using System;

namespace VideoOS.Common.Integration.Security;

public interface ILegacySecurityAttributeMapper
{
	LegacySecurityActionInformation GetSecurityActionInformation(string legacySecurityAttributeId);

	LegacySecurityActionInformation GetSecurityActionInformation(Guid namespaceId, string securityAction);
}

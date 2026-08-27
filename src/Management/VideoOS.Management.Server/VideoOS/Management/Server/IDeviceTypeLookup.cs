using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IDeviceTypeLookup
{
	Guid QueryDeviceSecurityNamespaceId(IGeminiRequestContext requestContext, string deviceId);

	DeviceType QueryDeviceTypeFromDeviceId(IGeminiRequestContext requestContext, string deviceId);

	DeviceType QueryDeviceTypeFromSecurityAttributeValue(string securityAttributeValue);
}

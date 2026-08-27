using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace VideoOS.ServiceHost.Common;

public interface IWebServiceUtils
{
	WSHttpBinding CreateWSHttpBinding(SecurityMode securityMode, TimeSpan receiveTimeout, WSMessageEncoding encoding, int maxMessageSize = 524288000);

	WSHttpBinding CreateWSHttpBinding(SecurityMode securityMode, TimeSpan receiveTimeout, MessageCredentialType messageCredentialType, int maxMessageSize = 524288000);

	BasicHttpBinding CreateBasicHttpBinding(TimeSpan receiveTimeout, int maxMessageSize = 524288000);

	ServiceThrottlingBehavior CreateServiceThrottlingBehavior();

	ServiceMetadataBehavior CreateServiceMetadataBehavior(SecurityMode securityMode);

	ServiceMetadataBehavior CreateServiceMetadataBehavior(bool isHttpsBased);
}

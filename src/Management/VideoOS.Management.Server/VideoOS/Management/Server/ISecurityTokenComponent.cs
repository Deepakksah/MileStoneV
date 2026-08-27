using System;
using VideoOS.Common.Integration.Security;

namespace VideoOS.Management.Server;

public interface ISecurityTokenComponent : IDisposable
{
	SecurityToken AddToken(string sid, TimeSpan timeout, bool managementClientToken);

	SecurityToken QueryToken(string tokenId);
}

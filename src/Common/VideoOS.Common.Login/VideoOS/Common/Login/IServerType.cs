using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public interface IServerType
{
	Guid ServerTypeId { get; }

	string ServerTypeName { get; }

	AuthenticationType DefaultAuthenticationType { get; }

	ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes { get; }
}

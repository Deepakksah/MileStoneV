using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Login;

public abstract class ServerType : IServerType
{
	public string ServerTypeName { get; private set; }

	public abstract Guid ServerTypeId { get; }

	public abstract AuthenticationType DefaultAuthenticationType { get; }

	public abstract ReadOnlyCollection<AuthenticationInfo> AuthenticationTypes { get; }

	protected ServerType(string serverTypeName)
	{
		ServerTypeName = serverTypeName;
	}

	public override string ToString()
	{
		return ServerTypeName;
	}
}

using System;
using System.Runtime.CompilerServices;

namespace VideoOS.Installer.Interfaces.Content;

public class ClientEndpoint
{
	public Guid Id { get; }

	public string DisplayNameKey { get; }

	public string RegisterImplementationType { get; }

	public Guid ServerGroupDependency { get; }

	public string SetServerAddressImplementationType { get; }

	public ClientEndpoint(Guid id, string displayNameKey, string registerImplementationType, string setServerAddressImplementationType, Guid serverGroupDependency)
	{
		if (registerImplementationType == null && setServerAddressImplementationType == null)
		{
			throw new InvalidOperationException("All registrars are null");
		}
		if (!(id != Guid.Empty))
		{
			throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("{0} cannot be Guid.Empty", "id")));
		}
		Id = id;
		DisplayNameKey = displayNameKey;
		RegisterImplementationType = registerImplementationType;
		SetServerAddressImplementationType = setServerAddressImplementationType;
		ServerGroupDependency = serverGroupDependency;
	}
}

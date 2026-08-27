using System;

namespace VideoOS.Management.VmoClient;

public interface ISite
{
	Guid Id { get; }

	string Name { get; }

	string Description { get; }

	DateTime LastModified { get; }

	ReadOnlyAddressCollection Addresses { get; }

	ServerVersion SiteVersion { get; }
}

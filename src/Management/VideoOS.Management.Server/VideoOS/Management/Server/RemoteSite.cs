using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public class RemoteSite : ISite
{
	public Guid Id { get; private set; }

	public string Name => "";

	public string Description => "";

	public DateTime LastModified => DateTime.MinValue;

	public ReadOnlyAddressCollection Addresses { get; private set; }

	public ServerVersion SiteVersion { get; private set; }

	public RemoteSite(string id, ServerAddress[] addresses, ServerVersion siteVersion)
	{
		Id = new Guid(id);
		ReadOnlyAddressCollection readOnlyAddressCollection = new ReadOnlyAddressCollection();
		foreach (ServerAddress serverAddress in addresses)
		{
			readOnlyAddressCollection.Add(new ReadOnlyAddress(new Uri(serverAddress.URL), (AddressType)serverAddress.Type));
		}
		Addresses = readOnlyAddressCollection;
		SiteVersion = siteVersion;
	}
}

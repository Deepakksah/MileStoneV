using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Server : IConfigurationItem
{
	private ObservableCollection<string> _Addresses;

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public ObservableCollection<string> Addresses
	{
		get
		{
			if (_Addresses == null)
			{
				_Addresses = base.Wrapper.GetPropertyArrayString("Addresses");
				_Addresses.CollectionChanged += _Addresses_CollectionChanged;
			}
			return _Addresses;
		}
	}

	public string Type => base.Wrapper.GetPropertyValue("Type");

	public Server(ServerId serverId, ConfigurationItem server)
		: base(serverId, server)
	{
	}

	public Server(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Server(FQID fqid)
		: base(fqid)
	{
	}

	private void _Addresses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("Addresses", _Addresses);
	}
}

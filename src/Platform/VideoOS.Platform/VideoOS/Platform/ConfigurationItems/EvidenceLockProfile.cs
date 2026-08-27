using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EvidenceLockProfile : IConfigurationItem
{
	private ObservableCollection<string> _RetentionTimeOptions;

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public ObservableCollection<string> RetentionTimeOptions
	{
		get
		{
			if (_RetentionTimeOptions == null)
			{
				_RetentionTimeOptions = base.Wrapper.GetPropertyArrayString("RetentionTimeOptions");
				_RetentionTimeOptions.CollectionChanged += _RetentionTimeOptions_CollectionChanged;
			}
			return _RetentionTimeOptions;
		}
	}

	public EvidenceLockProfile(ServerId serverId, ConfigurationItem evidenceLockProfile)
		: base(serverId, evidenceLockProfile)
	{
	}

	public EvidenceLockProfile(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EvidenceLockProfile(FQID fqid)
		: base(fqid)
	{
	}

	private void _RetentionTimeOptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("RetentionTimeOptions", _RetentionTimeOptions);
	}
}

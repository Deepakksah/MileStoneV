using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StateGroup : IConfigurationItem
{
	private ObservableCollection<string> _States;

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public ObservableCollection<string> States
	{
		get
		{
			if (_States == null)
			{
				_States = base.Wrapper.GetPropertyArrayString("States");
				_States.CollectionChanged += _States_CollectionChanged;
			}
			return _States;
		}
	}

	public StateGroup(ServerId serverId, ConfigurationItem stateGroup)
		: base(serverId, stateGroup)
	{
	}

	public StateGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StateGroup(FQID fqid)
		: base(fqid)
	{
	}

	private void _States_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("States", _States);
	}
}

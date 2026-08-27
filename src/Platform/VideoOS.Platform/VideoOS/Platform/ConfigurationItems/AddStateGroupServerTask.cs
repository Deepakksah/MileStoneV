using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddStateGroupServerTask : ServerTask
{
	private ObservableCollection<string> _States;

	public string Id
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Id");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Id", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
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

	public AddStateGroupServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddStateGroup"));
	}

	private void _States_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("States", _States);
	}
}

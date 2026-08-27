using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddEvidenceLockProfileServerTask : ServerTask
{
	private ObservableCollection<string> _RetentionTimeOptions;

	public string Id => base.Wrapper.GetPropertyValue("Id");

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

	public AddEvidenceLockProfileServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddEvidenceLockProfile"));
	}

	private void _RetentionTimeOptions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("RetentionTimeOptions", _RetentionTimeOptions);
	}
}

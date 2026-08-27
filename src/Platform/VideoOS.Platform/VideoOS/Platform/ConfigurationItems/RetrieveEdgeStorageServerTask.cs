using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RetrieveEdgeStorageServerTask : ServerTask
{
	private ObservableCollection<string> _RelatedTasks;

	public DateTime StartTime
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("startTime"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("startTime", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime EndTime
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("endTime"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
		set
		{
			base.Wrapper.SetPropertyValue("endTime", TimeConvert.ToGmtString(value));
			base.Wrapper.IsDirty = true;
		}
	}

	public string Identifier
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Identifier");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Identifier", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ObservableCollection<string> RelatedTasks
	{
		get
		{
			if (_RelatedTasks == null)
			{
				_RelatedTasks = base.Wrapper.GetPropertyArrayString("RelatedTasks");
				_RelatedTasks.CollectionChanged += _RelatedTasks_CollectionChanged;
			}
			return _RelatedTasks;
		}
	}

	public RetrieveEdgeStorageServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RetrieveEdgeStorage"));
	}

	private void _RelatedTasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("RelatedTasks", _RelatedTasks);
	}
}

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AnalyticsEvent : IConfigurationItem
{
	private ObservableCollection<string> _SourceArray;

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

	public ObservableCollection<string> SourceArray
	{
		get
		{
			if (_SourceArray == null)
			{
				_SourceArray = base.Wrapper.GetPropertyArrayString("SourceArray");
				_SourceArray.CollectionChanged += _SourceArray_CollectionChanged;
			}
			return _SourceArray;
		}
	}

	public AnalyticsEvent(ServerId serverId, ConfigurationItem analyticsEvent)
		: base(serverId, analyticsEvent)
	{
	}

	public AnalyticsEvent(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AnalyticsEvent(FQID fqid)
		: base(fqid)
	{
	}

	private void _SourceArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("SourceArray", _SourceArray);
	}
}

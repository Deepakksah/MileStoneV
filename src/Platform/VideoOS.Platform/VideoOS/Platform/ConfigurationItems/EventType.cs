using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class EventType : IConfigurationItem
{
	private ObservableCollection<string> _SourceArray;

	private ObservableCollection<string> _SourceFilterArray;

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

	public string GeneratorType => base.Wrapper.GetPropertyValue("GeneratorType");

	public string GeneratorSubtype => base.Wrapper.GetPropertyValue("GeneratorSubtype");

	public string GeneratorName => base.Wrapper.GetPropertyValue("GeneratorName");

	public string GeneratorID => base.Wrapper.GetPropertyValue("GeneratorID");

	public string GeneratorGroupName => base.Wrapper.GetPropertyValue("GeneratorGroupName");

	public string GeneratorGroupId => base.Wrapper.GetPropertyValue("GeneratorGroupId");

	public bool OccursGlobally
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("OccursGlobally");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public bool BuiltIn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BuiltIn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public string CounterEventID => base.Wrapper.GetPropertyValue("CounterEventID");

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

	public ObservableCollection<string> SourceFilterArray
	{
		get
		{
			if (_SourceFilterArray == null)
			{
				_SourceFilterArray = base.Wrapper.GetPropertyArrayString("SourceFilterArray");
				_SourceFilterArray.CollectionChanged += _SourceFilterArray_CollectionChanged;
			}
			return _SourceFilterArray;
		}
	}

	public string StateGroupId => base.Wrapper.GetPropertyValue("StateGroupId");

	public string State => base.Wrapper.GetPropertyValue("State");

	public EventType(ServerId serverId, ConfigurationItem eventType)
		: base(serverId, eventType)
	{
	}

	public EventType(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public EventType(FQID fqid)
		: base(fqid)
	{
	}

	private void _SourceArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("SourceArray", _SourceArray);
	}

	private void _SourceFilterArray_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("SourceFilterArray", _SourceFilterArray);
	}
}

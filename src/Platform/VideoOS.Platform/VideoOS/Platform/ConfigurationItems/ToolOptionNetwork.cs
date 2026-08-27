using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionNetwork : IConfigurationItem
{
	private ObservableCollection<string> _NetworkRangeList;

	public ObservableCollection<string> NetworkRangeList
	{
		get
		{
			if (_NetworkRangeList == null)
			{
				_NetworkRangeList = base.Wrapper.GetPropertyArrayString("NetworkRangeList");
				_NetworkRangeList.CollectionChanged += _NetworkRangeList_CollectionChanged;
			}
			return _NetworkRangeList;
		}
	}

	public bool ServerRecorderIPv6Comm
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ServerRecorderIPv6Comm");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ServerRecorderIPv6Comm", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string ServerLanAddress
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ServerLanAddress");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ServerLanAddress", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ServerWanAddress
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ServerWanAddress");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ServerWanAddress", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionNetwork(ServerId serverId, ConfigurationItem toolOptionNetwork)
		: base(serverId, toolOptionNetwork)
	{
	}

	public ToolOptionNetwork(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionNetwork(FQID fqid)
		: base(fqid)
	{
	}

	private void _NetworkRangeList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
	{
		base.Wrapper.SetPropertyArrayString("NetworkRangeList", _NetworkRangeList);
	}
}

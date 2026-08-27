using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;

namespace VideoOS.Management.Server.ConfigurationApi;

public class ChildrenCache : IDisposable
{
	private Dictionary<string, List<ConfigurationItem>> _parentList = new Dictionary<string, List<ConfigurationItem>>();

	private List<string> _loadedItemTypes = new List<string>();

	private int _count;

	private bool disposedValue;

	public int Count => _count;

	public List<string> LoadedItemTypes => _loadedItemTypes;

	private string topLevelId { get; set; }

	public ChildrenCache()
	{
	}

	public bool IsEmpty()
	{
		return !_parentList.Any();
	}

	public void Add(ConfigurationItem item)
	{
		if (!_parentList.ContainsKey(item.ParentPath.ToUpperInvariant()))
		{
			_parentList.Add(item.ParentPath.ToUpperInvariant(), new List<ConfigurationItem>());
		}
		_parentList[item.ParentPath.ToUpperInvariant()].Add(item);
		_count++;
	}

	public void AddRange(IEnumerable<ConfigurationItem> items)
	{
		foreach (ConfigurationItem item in items)
		{
			Add(item);
		}
	}

	public void AddLoadedItemType(string itemType)
	{
		_loadedItemTypes.Add(itemType);
	}

	public List<ConfigurationItem> GetChildrenForParent(string parentPath)
	{
		if (_parentList.TryGetValue(parentPath.ToUpperInvariant(), out var value))
		{
			return value;
		}
		return new List<ConfigurationItem>();
	}

	public void CheckLoad(IGeminiRequestContext requestContext, ConfigurationItem[] configurationItems, string[] itemTypes, ItemFilter[] itemFilters)
	{
		if (!configurationItems.Any())
		{
			return;
		}
		topLevelId = HaveSameParentId(configurationItems, out var itemTypes2, out var itemTypeForId);
		if (!IsEmpty())
		{
			return;
		}
		foreach (string item in itemTypes2)
		{
			switch (item)
			{
			case "RecordingServer":
			case "Hardware":
				if (itemTypes.Contains("Camera") || itemTypes.Contains("Microphone") || itemTypes.Contains("Speaker") || itemTypes.Contains("InputEvent") || itemTypes.Contains("Output") || itemTypes.Contains("Metadata"))
				{
					LoadDevices(requestContext, itemTypes, itemFilters, itemTypeForId, topLevelId);
				}
				continue;
			case "ViewGroup":
				if (itemTypes.Contains("ViewGroup") || itemTypes.Contains("View"))
				{
					LoadAllSubGroupsAndViews(requestContext, itemTypes);
					continue;
				}
				break;
			}
			if (item == "AccessControlUnit" && itemTypes.Contains("AccessControlUnit"))
			{
				LoadAllAccessControlUnits(requestContext);
			}
		}
	}

	private static string HaveSameParentId(ConfigurationItem[] configurationItems, out HashSet<string> itemTypes, out string itemTypeForId)
	{
		string text = null;
		itemTypeForId = null;
		itemTypes = new HashSet<string>();
		foreach (ConfigurationItem configurationItem in configurationItems)
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(configurationItem.Path);
			if (string.IsNullOrEmpty(configurationItemPath.Id))
			{
				return null;
			}
			if (text == null)
			{
				text = configurationItemPath.Id;
				if (configurationItemPath.IsFolder)
				{
					itemTypeForId = configurationItemPath.ParentItemType;
				}
				else
				{
					itemTypeForId = configurationItemPath.ItemType;
				}
			}
			else if (text != configurationItemPath.Id)
			{
				return null;
			}
			itemTypes.Add(ConfigurationApiBase.RemoveFolderPartFromItemType(configurationItem.ItemType));
		}
		return text;
	}

	private void LoadDevices(IGeminiRequestContext requestContext, string[] itemTypes, ItemFilter[] itemFilters, string itemTypeForId, string topLevelId)
	{
		IConfigurationApiSupport configurationApiSupport = null;
		try
		{
			configurationApiSupport = ConfigurationApiObjectDispatcher.GetObject("Camera");
		}
		catch (NotSupportedException)
		{
			throw new PathNotFoundException("The server doesn't support the specified path: Camera");
		}
		foreach (string itemType in itemTypes)
		{
			if (_loadedItemTypes.Contains(itemType))
			{
				continue;
			}
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.ItemType == itemType);
			if (deviceTypeRelation != null)
			{
				ConfigurationItemPath folderPath = ConfigurationItemPath.CreatePath(string.Format(CultureInfo.InvariantCulture, "{0}[{1}]/{2}", deviceTypeRelation.GroupType, deviceTypeRelation.AllFolderId, deviceTypeRelation.FolderType));
				ItemFilter itemFilter = itemFilters.FirstOrDefault((ItemFilter f) => f.ItemType == itemType);
				if (itemFilter == null)
				{
					itemFilter = new ItemFilter(itemType, new PropertyFilter[0], EnableFilter.Enabled);
				}
				List<PropertyFilter> list = new List<PropertyFilter>(itemFilter.PropertyFilters);
				if (topLevelId != null)
				{
					PropertyFilter item = ((itemTypeForId == "RecordingServer") ? new PropertyFilter("IDRecorder", Operator.Equals, topLevelId) : new PropertyFilter("IDHardware", Operator.Equals, topLevelId));
					list.Add(item);
				}
				ItemFilter itemFilter2 = new ItemFilter(itemFilter.ItemType, list.ToArray(), itemFilter.EnableFilter);
				ConfigurationItem[] items = configurationApiSupport.QueryItems(requestContext, folderPath, itemFilter2, 999999, 0, null);
				AddRange(items);
				AddLoadedItemType(itemType);
			}
		}
	}

	private void LoadAllSubGroupsAndViews(IGeminiRequestContext requestContext, string[] itemTypes)
	{
		if ((!_loadedItemTypes.Contains("ViewGroup") && itemTypes.Contains("ViewGroup")) || itemTypes.Contains("ViewGroupFolder"))
		{
			ServerGroup[] array = ((topLevelId == null) ? ServerGroup.QueryServerGroupAllNoSecurity(requestContext, ServerGroup.EnumStructureType.ViewGroup) : ServerGroup.QueryServerGroupHeriarchyByIdNoSecurity(requestContext, topLevelId));
			foreach (ServerGroup serverGroup in array)
			{
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath("ViewGroup", serverGroup.ParentId, "ViewGroupFolder");
				Add(ViewGroupConfigurationApi.ConvertToConfigurationItem(parentPath, serverGroup));
			}
			AddLoadedItemType("ViewGroup");
		}
		if ((_loadedItemTypes.Contains("View") || !itemTypes.Contains("View")) && !itemTypes.Contains("ViewFolder"))
		{
			return;
		}
		View[] obj = ((topLevelId == null) ? View.QueryViewAllNoSecurity(requestContext) : View.QueryViewHeriarchyByServerGroupIdNoSecurity(requestContext, topLevelId));
		ViewItem[] source = ((topLevelId == null) ? ViewItem.QueryViewItemsAll(requestContext) : View.QueryViewItemHeriarchyByServerGroupIdNoSecurity(requestContext, topLevelId));
		View[] array2 = obj;
		foreach (View view in array2)
		{
			IEnumerable<ViewItem> source2 = source.Where((ViewItem v) => v.ViewId == view.Id);
			view.ViewItems = source2.ToArray();
			ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath("ViewGroup", view.ViewGroupId, "ViewFolder");
			Add(ViewConfigurationApi.ConvertToViewConfigurationItem(parentPath2, view));
		}
		AddLoadedItemType("View");
	}

	private void LoadAllAccessControlUnits(IGeminiRequestContext requestContext)
	{
		ConfigurationItemBuilder configurationItemBuilder = new ConfigurationItemBuilder();
		string id = Guid.Empty.ToString();
		MIPObject[] array = MIPObject.QueryMIPObjectsByKind(requestContext, "6d3bdb33-d913-470f-8784-3dd4ee94db8c", null);
		foreach (MIPObject mIPObject in array)
		{
			ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("AccessControlUnit", mIPObject.Id);
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath("AccessControlUnit", id), "AccessControlUnitFolder");
			Add(configurationItemBuilder.BuildAccessControlItem(path, parentPath, mIPObject.Name, mIPObject, Array.Empty<string>()));
		}
		AddLoadedItemType("AccessControlUnit");
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_parentList.Clear();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Admin;

public class ItemManager
{
	public struct ItemSummaryHeader
	{
		public string Name;

		public Type Type;

		public bool ReadOnly;
	}

	public enum AdminTabBehaviour
	{
		Group,
		Individual
	}

	private Item _currentItem;

	private AdminTabBehaviour _tabBehaviour;

	private int _summaryUpdateInterval;

	public bool ExcludePersonalDataFromConfigReport { get; set; }

	public Item CurrentItem
	{
		get
		{
			return _currentItem;
		}
		set
		{
			_currentItem = value;
		}
	}

	public virtual bool DisableUserControlWhenNoItemIsSelected => true;

	public virtual int SummaryUpdateInterval
	{
		get
		{
			return _summaryUpdateInterval;
		}
		set
		{
			_summaryUpdateInterval = value;
		}
	}

	public virtual AdminTabBehaviour TabBehaviour
	{
		get
		{
			return _tabBehaviour;
		}
		set
		{
			_tabBehaviour = value;
		}
	}

	public event EventHandler ConfigurationChangedByUser;

	private event EventHandler ConfigurationChangedByUserPushed;

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public void ConfigurationChangedByUserHandler(object sender, EventArgs e)
	{
		if (this.ConfigurationChangedByUser != null)
		{
			this.ConfigurationChangedByUser(sender, e);
		}
	}

	public void PushConfigurationChangedEventHandler()
	{
		this.ConfigurationChangedByUserPushed = this.ConfigurationChangedByUser;
		this.ConfigurationChangedByUser = null;
	}

	public void PopConfigurationChangedEventHandler()
	{
		this.ConfigurationChangedByUser = this.ConfigurationChangedByUserPushed;
		this.ConfigurationChangedByUserPushed = null;
	}

	public virtual UserControl GenerateDetailUserControl()
	{
		return new UserControl();
	}

	public virtual List<DetailedUserControl> GenerateDetailUserControlList()
	{
		return new List<DetailedUserControl>();
	}

	public virtual ItemNodeUserControl GenerateOverviewUserControl()
	{
		return new ItemNodeUserControl();
	}

	public virtual void ReleaseUserControl()
	{
	}

	public virtual UserControl GenerateAddUserControl(Item parentItem, FQID suggestedFQID)
	{
		return null;
	}

	public virtual Dictionary<string, string> GetTranslationDictionary(CultureInfo culture)
	{
		return null;
	}

	public virtual bool ValidateAddUserControl(UserControl addUserControl)
	{
		return true;
	}

	public virtual void FillUserControl(Item item)
	{
		_currentItem = item;
		if (item != null)
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name + ".FillUserControl", "Not implemented - UI may not be correct", null);
		}
	}

	public virtual void FillUserControl()
	{
		try
		{
			FillUserControl(null);
		}
		catch (NullReferenceException)
		{
		}
	}

	public virtual void ClearUserControl()
	{
		_currentItem = null;
		EnvironmentManager.Instance.Log(error: true, GetType().Name + ".ClearUserControl", "Not implemented - UI may not be correct", null);
	}

	public virtual void SetItemName(string name)
	{
		_currentItem.Name = name;
	}

	public virtual string GetItemName()
	{
		return _currentItem.Name;
	}

	public virtual List<Item> GetItems()
	{
		return new List<Item>();
	}

	public virtual List<Item> GetItems(Item parentItem)
	{
		if (parentItem == null)
		{
			return GetItems();
		}
		return new List<Item>();
	}

	public virtual Item GetItem(FQID fqid)
	{
		return null;
	}

	public virtual bool ValidateAndSaveUserControl()
	{
		return true;
	}

	public virtual bool ValidateAndSaveItemSummary(Item item, List<object> allValues, List<int> changedIndices, out int errIndex)
	{
		errIndex = -1;
		return true;
	}

	public virtual List<ItemSummaryHeader> GetItemSummaryHeaders()
	{
		return new List<ItemSummaryHeader>();
	}

	public virtual List<object> GetItemSummary(Item item)
	{
		return new List<object>();
	}

	public virtual Dictionary<string, string> GetConfigurationReportProperties(Item item)
	{
		return null;
	}

	public virtual bool IsContextMenuValid(string command)
	{
		return true;
	}

	public virtual Collection<ContextMenuItem> GetContextMenu(Item item = null)
	{
		return new Collection<ContextMenuItem>();
	}

	public virtual Item CreateItem(Item parentItem, FQID suggestedFQID)
	{
		throw new NotImplementedException("CreateItem not implemented");
	}

	public virtual Item CreateItem(Item parentItem, FQID suggestedFQID, UserControl addUserControl)
	{
		throw new NotImplementedException("CreateItem not implemented");
	}

	public virtual void DeleteItem(Item item)
	{
		throw new NotImplementedException("DeleteItem not implemented");
	}

	[Obsolete("Do not use this method. Use GetKnownEventTypes(CultureInfo culture)")]
	public virtual Collection<string> GetKnownEventTypes()
	{
		return new Collection<string>();
	}

	public virtual Collection<EventType> GetKnownEventTypes(CultureInfo culture)
	{
		return null;
	}

	public virtual Collection<EventGroup> GetKnownEventGroups(CultureInfo culture)
	{
		return new Collection<EventGroup>();
	}

	public virtual Collection<StateGroup> GetKnownStateGroups(CultureInfo culture)
	{
		return new Collection<StateGroup>();
	}

	public virtual OperationalState GetOperationalState(Item item)
	{
		return OperationalState.Ok;
	}

	public virtual string GetItemStatusDetails(Item item, string language)
	{
		return GetItemStatusDetails(item);
	}

	[Obsolete("Do not use this method. Overwrite GetItemStatusDetails(Item item, string language) instead")]
	public virtual string GetItemStatusDetails(Item item)
	{
		return null;
	}

	public virtual UserControl GenerateAlarmPreviewUserControl(FQID fqid, object alarmOrBaseEvent)
	{
		return null;
	}
}

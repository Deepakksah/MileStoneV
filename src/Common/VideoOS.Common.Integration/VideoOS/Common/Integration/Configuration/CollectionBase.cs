using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public abstract class CollectionBase : DictionaryBase
{
	protected ConfigurationManager _configurationManager;

	protected DataTable _table;

	private bool _modified;

	private List<IConfigurationItem> _deletedItems;

	public IConfigurationItem this[Guid id] => (IConfigurationItem)base.Dictionary[id];

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public CollectionBase(ConfigurationManager manager, DataTable table)
	{
		_configurationManager = manager;
		_deletedItems = new List<IConfigurationItem>();
		_configurationManager.ChangesAccepted += _configurationManager_ChangesAccepted;
		_configurationManager.ChangesRejected += _configurationManager_ChangesRejected;
		_table = table;
		_configurationManager.EnsureTableDataLoaded(_table);
		foreach (DataRow row in _table.Rows)
		{
			AddInternal(row);
		}
		_modified = false;
	}

	public new IEnumerator GetEnumerator()
	{
		return base.Dictionary.Values.GetEnumerator();
	}

	public bool Contains(Guid id)
	{
		return base.Dictionary.Contains(id);
	}

	protected abstract IConfigurationItem GetInstance(DataRow row);

	protected virtual IConfigurationItem Add(DataRow row)
	{
		return AddInternal(row);
	}

	protected IConfigurationItem AddInternal(DataRow row)
	{
		IConfigurationItem instance = GetInstance(row);
		base.Dictionary.Add(instance.ID, instance);
		_modified = true;
		return instance;
	}

	public virtual void Remove(Guid id)
	{
		IConfigurationItem configurationItem = this[id];
		if (configurationItem != null)
		{
			base.Dictionary.Remove(id);
			configurationItem.DataRow.Delete();
			_deletedItems.Add(configurationItem);
			_modified = true;
		}
	}

	public new void Clear()
	{
		ArrayList arrayList = new ArrayList();
		foreach (IConfigurationItem value in Values)
		{
			arrayList.Add(value.ID);
		}
		foreach (Guid item in arrayList)
		{
			Remove(item);
		}
	}

	private void _configurationManager_ChangesRejected(object sender, EventArgs e)
	{
		if (!_modified)
		{
			return;
		}
		List<Guid> list = new List<Guid>();
		foreach (DictionaryEntry item in base.Dictionary)
		{
			if (((IConfigurationItem)item.Value).DataRow.RowState == DataRowState.Detached)
			{
				list.Add((Guid)item.Key);
			}
		}
		list.ForEach(delegate(Guid i)
		{
			base.Dictionary.Remove(i);
		});
		_deletedItems.ForEach(delegate(IConfigurationItem di)
		{
			base.Dictionary.Add(di.ID, di);
		});
		_deletedItems.Clear();
		_modified = false;
	}

	private void _configurationManager_ChangesAccepted(object sender, EventArgs e)
	{
		if (_modified)
		{
			_deletedItems.Clear();
			_modified = false;
		}
	}
}

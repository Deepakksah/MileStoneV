using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace VideoOS.Common.Integration.Configuration;

public abstract class CollectionBaseString : DictionaryBase
{
	protected ConfigurationManager _configurationManager;

	protected DataTable _table;

	private bool _modified;

	private List<IConfigurationItemString> _deletedItems;

	public IConfigurationItemString this[string id] => (IConfigurationItemString)base.Dictionary[id];

	public ICollection Keys => base.Dictionary.Keys;

	public ICollection Values => base.Dictionary.Values;

	public CollectionBaseString(ConfigurationManager manager, DataTable table)
	{
		_configurationManager = manager;
		_deletedItems = new List<IConfigurationItemString>();
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

	public bool Contains(string id)
	{
		return base.Dictionary.Contains(id);
	}

	protected abstract IConfigurationItemString GetInstance(DataRow row);

	protected virtual IConfigurationItemString Add(DataRow row)
	{
		return AddInternal(row);
	}

	protected IConfigurationItemString AddInternal(DataRow row)
	{
		IConfigurationItemString instance = GetInstance(row);
		base.Dictionary.Add(instance.ID, instance);
		_modified = true;
		return instance;
	}

	public virtual void Remove(string id)
	{
		IConfigurationItemString configurationItemString = this[id];
		if (configurationItemString != null)
		{
			base.Dictionary.Remove(id);
			configurationItemString.DataRow.Delete();
			_deletedItems.Add(configurationItemString);
			_modified = true;
		}
	}

	public new void Clear()
	{
		ArrayList arrayList = new ArrayList();
		foreach (IConfigurationItemString value in Values)
		{
			arrayList.Add(value.ID);
		}
		foreach (string item in arrayList)
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
		List<string> list = new List<string>();
		foreach (DictionaryEntry item in base.Dictionary)
		{
			if (((IConfigurationItemString)item.Value).DataRow.RowState == DataRowState.Detached)
			{
				list.Add((string)item.Key);
			}
		}
		list.ForEach(delegate(string i)
		{
			base.Dictionary.Remove(i);
		});
		_deletedItems.ForEach(delegate(IConfigurationItemString di)
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

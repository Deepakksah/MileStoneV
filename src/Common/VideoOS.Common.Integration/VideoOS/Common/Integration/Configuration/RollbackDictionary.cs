using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

[DebuggerDisplay("{_parent.GetType().Name} ({Count})")]
public class RollbackDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private bool _modified;

	private Dictionary<TKey, TValue> _deletedItems;

	private object _parent;

	public RollbackDictionary(ConfigurationManager configurationManager, object parent)
	{
		_configurationManager = configurationManager;
		_parent = parent;
		_deletedItems = new Dictionary<TKey, TValue>();
		_configurationManager.ChangesAccepted += _configurationManager_ChangesAccepted;
		_configurationManager.ChangesRejected += _configurationManager_ChangesRejected;
		_modified = false;
	}

	public new void Add(TKey key, TValue value)
	{
		base.Add(key, value);
		_modified = true;
	}

	public new bool Remove(TKey key)
	{
		TValue val = base[key];
		if (val != null)
		{
			_modified = true;
			_deletedItems.Add(key, val);
			return base.Remove(key);
		}
		return false;
	}

	public new void Clear()
	{
		base.Keys.ToList().ForEach(delegate(TKey k)
		{
			Remove(k);
		});
	}

	private void _configurationManager_ChangesRejected(object sender, EventArgs e)
	{
		if (!_modified)
		{
			return;
		}
		List<TKey> list = new List<TKey>();
		using (Enumerator enumerator = GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<TKey, TValue> current = enumerator.Current;
				if (current.Value.DataRow.RowState == DataRowState.Detached)
				{
					list.Add(current.Key);
				}
			}
		}
		list.ForEach(delegate(TKey i)
		{
			base.Remove(i);
		});
		foreach (KeyValuePair<TKey, TValue> deletedItem in _deletedItems)
		{
			base.Add(deletedItem.Key, deletedItem.Value);
		}
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

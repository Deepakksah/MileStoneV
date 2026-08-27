using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Management.VmoClient;

public abstract class VmoCollection<T> : VmoCollectionBase, IEnumerable<T>, IEnumerable where T : VmoObject
{
	public delegate T[] LoadMethod(bool refreshCachedObjects);

	private Func<bool, T[]> _loadData;

	public Dictionary<string, T> _internalStorage = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

	protected VmoObject Parent { get; private set; }

	public bool IsInitialized { get; private set; }

	public int Count
	{
		get
		{
			CheckForDataLoaded();
			return _internalStorage.Count;
		}
	}

	public override Type ContentType => typeof(T);

	public VmoCollection(Func<bool, T[]> loadData, VmoObject parent, bool isInitialized)
	{
		_loadData = loadData;
		IsInitialized = isInitialized;
		parent.RegisterCollection(typeof(T), this);
		Parent = parent;
	}

	public virtual void Refresh(bool refreshChildObjects)
	{
		LoadData(refreshChildObjects);
	}

	public void Add(T value)
	{
		if (IsInitialized)
		{
			T value2 = null;
			_internalStorage.TryGetValue(value.InternalId, out value2);
			if (value2 == null)
			{
				_internalStorage.Add(value.InternalId, value);
				value.Deleted += ObjectDeleted;
			}
		}
	}

	public void Remove(string internalId)
	{
		if (IsInitialized && _internalStorage.ContainsKey(internalId))
		{
			_internalStorage[internalId].Deleted -= ObjectDeleted;
			_internalStorage.Remove(internalId);
		}
	}

	private void ObjectDeleted(object sender, EventArgs e)
	{
		Remove(((VmoObject)sender).InternalId);
	}

	private void CheckForDataLoaded()
	{
		if (!IsInitialized)
		{
			LoadData(refreshChildObjects: false);
		}
	}

	protected T GetObjectByIndex(string index)
	{
		CheckForDataLoaded();
		T value = null;
		_internalStorage.TryGetValue(index, out value);
		return value;
	}

	private void LoadData(bool refreshChildObjects)
	{
		T[] array = _loadData(refreshChildObjects);
		_internalStorage.Clear();
		IsInitialized = true;
		if (array != null)
		{
			T[] array2 = array;
			foreach (T value in array2)
			{
				Add(value);
			}
		}
	}

	public override void AddVmoObject(VmoObject vmoObject)
	{
		Add((T)vmoObject);
	}

	public override void RemoveVmoObject(VmoObject vmoObject)
	{
		Remove(vmoObject.InternalId);
	}

	public IEnumerator<T> GetEnumerator()
	{
		CheckForDataLoaded();
		return new Dictionary<string, T>.ValueCollection(_internalStorage).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		CheckForDataLoaded();
		return new Dictionary<string, T>.ValueCollection(_internalStorage).GetEnumerator();
	}
}

using System;
using System.Collections.Generic;

namespace VideoOS.UI.Common.WPF.Utils;

public class LiveInstanceCollection<T> where T : class
{
	private readonly List<WeakReference<T>> _instanceList = new List<WeakReference<T>>();

	private int _createdCount;

	public int CreatedCount => _createdCount;

	public int LiveInstanceCount
	{
		get
		{
			int num = 0;
			lock (_instanceList)
			{
				CleanupWithLockAcquired();
				return _instanceList.Count;
			}
		}
	}

	public void Register(T instance)
	{
		lock (_instanceList)
		{
			CleanupWithLockAcquired();
			_instanceList.Add(new WeakReference<T>(instance));
			_createdCount++;
		}
	}

	public IEnumerable<T> GetInstances()
	{
		WeakReference<T>[] array;
		lock (_instanceList)
		{
			CleanupWithLockAcquired();
			array = _instanceList.ToArray();
		}
		WeakReference<T>[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i].TryGetTarget(out var target))
			{
				yield return target;
			}
		}
	}

	private void Cleanup()
	{
		lock (_instanceList)
		{
			CleanupWithLockAcquired();
		}
	}

	private void CleanupWithLockAcquired()
	{
		_instanceList.RemoveAll((WeakReference<T> x) => !x.TryGetTarget(out var _));
	}
}

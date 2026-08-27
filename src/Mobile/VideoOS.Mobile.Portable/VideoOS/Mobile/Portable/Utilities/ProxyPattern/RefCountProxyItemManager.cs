using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Interfaces;

namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public class RefCountProxyItemManager<TI, TKey> : IRefCountProxyItemManager<TI, TKey>, IControl, IDisposable
{
	private readonly Dictionary<TKey, ItemDataHolder<TI, TKey>> _proxies = new Dictionary<TKey, ItemDataHolder<TI, TKey>>();

	private readonly object _proxiesLocker = new object();

	private readonly Func<TKey, TI> _itemFactoryFunc;

	private readonly Func<TI, ProxyItem<TI>> _proxyFactoryFunc;

	public RefCountProxyItemManager(Func<TKey, TI> itemFactoryFunc, Func<TI, ProxyItem<TI>> proxyFactoryFunc)
	{
		_itemFactoryFunc = itemFactoryFunc;
		_proxyFactoryFunc = proxyFactoryFunc;
	}

	public TI GetItem(TKey key)
	{
		lock (_proxiesLocker)
		{
			if (!_proxies.ContainsKey(key))
			{
				_proxies.Add(key, new ItemDataHolder<TI, TKey>
				{
					Key = key,
					ItemToBeProxied = _itemFactoryFunc(key)
				});
			}
			ItemDataHolder<TI, TKey> itemDataHolder = _proxies[key];
			ProxyItem<TI> proxyItem = _proxyFactoryFunc(itemDataHolder.ItemToBeProxied);
			itemDataHolder.Proxies.Add(new ProxyItemHolder<TI, TKey>(proxyItem, key, OnProxyDestruction));
			return proxyItem.GetProxy;
		}
	}

	private void OnProxyDestruction(TKey key, ProxyItem<TI> proxyItem)
	{
		TI val = default(TI);
		lock (_proxiesLocker)
		{
			if (!_proxies.ContainsKey(key))
			{
				return;
			}
			ItemDataHolder<TI, TKey> itemDataHolder = _proxies[key];
			itemDataHolder.Proxies.RemoveAll((ProxyItemHolder<TI, TKey> holder) => holder.Item == proxyItem);
			if (!itemDataHolder.Proxies.Any())
			{
				val = itemDataHolder.ItemToBeProxied;
				_proxies.Remove(key);
			}
		}
		(val as IControl)?.Stop();
		(val as IDisposable)?.Dispose();
	}

	public void Start()
	{
	}

	public void Stop()
	{
		ClearItems();
	}

	private void ClearItems()
	{
		IEnumerable<ItemDataHolder<TI, TKey>> enumerable;
		lock (_proxiesLocker)
		{
			enumerable = _proxies.Values.ToArray();
		}
		foreach (ItemDataHolder<TI, TKey> item in enumerable)
		{
			foreach (ProxyItemHolder<TI, TKey> proxy in item.Proxies)
			{
				proxy.Item.Dispose();
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			ClearItems();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~RefCountProxyItemManager()
	{
		Dispose(disposing: false);
	}
}

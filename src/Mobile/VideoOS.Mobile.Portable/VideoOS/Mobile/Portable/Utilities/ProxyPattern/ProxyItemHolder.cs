using System;

namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public class ProxyItemHolder<TI, TKey>
{
	public ProxyItem<TI> Item { get; }

	public TKey Key { get; }

	public Action<TKey, ProxyItem<TI>> OnProxyDestruction { get; }

	public ProxyItemHolder(ProxyItem<TI> item, TKey key, Action<TKey, ProxyItem<TI>> onProxyDestruction)
	{
		Item = item;
		Key = key;
		OnProxyDestruction = onProxyDestruction;
		Item.OnDestruction = OnDestruction;
	}

	private void OnDestruction(ProxyItem<TI> proxyItem)
	{
		OnProxyDestruction?.Invoke(Key, Item);
	}
}

using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public class ItemDataHolder<TI, TKey>
{
	public readonly List<ProxyItemHolder<TI, TKey>> Proxies = new List<ProxyItemHolder<TI, TKey>>();

	public TI ItemToBeProxied { get; set; }

	public TKey Key { get; set; }
}

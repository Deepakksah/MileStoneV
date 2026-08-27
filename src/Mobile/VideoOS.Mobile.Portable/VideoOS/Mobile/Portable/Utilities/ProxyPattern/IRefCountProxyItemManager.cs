namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public interface IRefCountProxyItemManager<out TI, in TKey>
{
	TI GetItem(TKey key);
}

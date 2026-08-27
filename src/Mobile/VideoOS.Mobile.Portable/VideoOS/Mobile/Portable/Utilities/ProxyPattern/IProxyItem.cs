namespace VideoOS.Mobile.Portable.Utilities.ProxyPattern;

public interface IProxyItem<out TI>
{
	TI GetProxy { get; }
}

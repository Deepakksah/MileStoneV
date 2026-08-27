namespace VideoOS.Central.Util;

public interface IDictTryGetValue<TKey, TValue>
{
	bool TryGetValue(TKey key, out TValue value);
}

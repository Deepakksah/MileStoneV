namespace VideoOS.Mobile.Utilities;

public interface IItemManagerAddRemove<in TKey, TItem>
{
	void Add(TKey key, TItem item);

	TItem RemoveItem(TKey key);
}

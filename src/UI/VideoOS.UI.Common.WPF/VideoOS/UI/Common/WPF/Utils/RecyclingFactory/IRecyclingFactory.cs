namespace VideoOS.UI.Common.WPF.Utils.RecyclingFactory;

public interface IRecyclingFactory<out TItem> where TItem : IRecyclableItem
{
	TItem Create();

	TItem Create(out bool created);

	void Clear();
}

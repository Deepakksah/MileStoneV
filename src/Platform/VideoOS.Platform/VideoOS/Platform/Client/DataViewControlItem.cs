namespace VideoOS.Platform.Client;

public class DataViewControlItem : ViewControlItem
{
	public string Data { get; }

	public DataViewControlItem(int indexInViewLayout, string data)
		: base(indexInViewLayout)
	{
		Data = data;
	}
}

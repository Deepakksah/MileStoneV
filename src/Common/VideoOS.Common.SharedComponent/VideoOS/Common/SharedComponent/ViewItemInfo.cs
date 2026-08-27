namespace VideoOS.Common.SharedComponent;

public class ViewItemInfo
{
	private ViewItemData _viewItem;

	private SizeAndLocation _viewItemsSizeAndLocation;

	public ViewItemData ViewItemData => _viewItem;

	public SizeAndLocation SizeAndLocation => _viewItemsSizeAndLocation;

	public ViewItemInfo()
	{
	}

	public void SetViewItemData(ViewItemData viewItemData)
	{
		_viewItem = viewItemData;
	}

	public void SetSizeAndLocation(SizeAndLocation sizeAndLocation)
	{
		_viewItemsSizeAndLocation = sizeAndLocation;
	}
}

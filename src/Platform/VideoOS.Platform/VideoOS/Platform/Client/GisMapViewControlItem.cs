namespace VideoOS.Platform.Client;

public class GisMapViewControlItem : ViewControlItem
{
	public bool AlarmAutoChange { get; set; }

	public bool IsInitialized { get; set; }

	public bool IsAutoMaximize { get; set; }

	public bool IsLocationSelectorVisible { get; set; }

	public bool IsBackButtonVisible { get; set; }

	public bool IsHomeButtonVisible { get; set; }

	public bool IsReCenterButtonVisible { get; set; }

	public int IconSize { get; set; }

	public bool ShowPreview { get; set; }

	public bool ShowStatus { get; set; }

	public bool ShowStatusDetails { get; set; }

	public GisMapViewControlItem(int indexInViewLayout)
		: base(indexInViewLayout)
	{
		AlarmAutoChange = true;
		base.ViewItemTypeId = ViewAndLayoutItem.GisMapBuiltinId;
		IsInitialized = false;
		IsAutoMaximize = true;
		IconSize = 0;
		ShowPreview = true;
		ShowStatus = true;
		ShowStatusDetails = true;
		IsLocationSelectorVisible = true;
		IsBackButtonVisible = true;
		IsHomeButtonVisible = true;
		IsReCenterButtonVisible = false;
	}
}

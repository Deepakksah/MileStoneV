using System;

namespace VideoOS.Platform.Client;

public class MapViewControlItem : ViewControlItem
{
	public Guid MapId { get; private set; }

	public bool AlarmAutoChange { get; set; }

	public bool IsInitialized { get; set; }

	public bool IsAutoMaximize { get; set; }

	public int TimeoutSeconds { get; set; }

	public int IconSize { get; set; }

	public bool ShowPreview { get; set; }

	public bool ShowStatus { get; set; }

	public bool ShowStatusDetails { get; set; }

	public MapViewControlItem(int indexInViewLayout, Guid mapId)
		: base(indexInViewLayout)
	{
		base.ViewItemTypeId = ViewAndLayoutItem.MapBuiltinId;
		MapId = mapId;
		AlarmAutoChange = false;
		IsInitialized = false;
		IsAutoMaximize = true;
		TimeoutSeconds = 30;
		IconSize = 0;
		ShowPreview = true;
		ShowStatus = true;
		ShowStatusDetails = true;
	}
}

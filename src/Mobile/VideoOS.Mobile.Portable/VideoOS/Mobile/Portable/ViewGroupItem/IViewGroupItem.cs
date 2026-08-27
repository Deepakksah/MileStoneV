using System;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

public interface IViewGroupItem
{
	ViewItemType ItemType { get; }

	string ItemName { get; }

	Guid ItemId { get; }

	Guid CameraId { get; }
}

using System;

namespace VideoOS.Mobile.Portable.OEGroupItem;

public interface IOEGroupTree
{
	OEItemType ItemType { get; }

	string ItemName { get; }

	Guid ObjectId { get; }

	Guid CameraId { get; }

	bool CanActivate { get; }
}

using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.ViewGroupItem;

public class CameraComparer : IComparer<IViewGroupItem>
{
	int IComparer<IViewGroupItem>.Compare(IViewGroupItem x, IViewGroupItem y)
	{
		return x.CameraId.CompareTo(y.CameraId);
	}
}

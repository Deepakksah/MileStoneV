using System;

namespace VideoOS.UI.Common.WPF.Utils.RecyclingFactory;

public interface IRecyclableItem
{
	event EventHandler ReadyToBeRecycled;
}

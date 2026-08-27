using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Controls;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class ViewControl : UserControl
{
	public List<Rectangle> ViewLayout { get; protected set; }

	public List<ViewControlItem> Items { get; protected set; }

	public WindowInformation ParentWindowInformation { get; private set; }

	public Guid WindowId { get; protected set; }

	public abstract bool Live { get; set; }

	public abstract DateTime PlaybackTime { get; set; }

	public virtual ViewControlItem SelectedItem { get; set; }

	public virtual bool EnabledViewItemToolbars { get; set; }

	public abstract event EventHandler<ViewControlItemArgs> SelectedItemChanged;

	public event EventHandler<ViewControlItemDataArgs> ItemDataChanged;

	protected ViewControl()
	{
		new TrackPageViewData(typeof(ViewControl)).Send();
	}

	protected ViewControl(List<ViewControlItem> items, List<Rectangle> viewLayout)
	{
		Items = items;
		ViewLayout = viewLayout;
	}

	protected ViewControl(List<ViewControlItem> items, List<Rectangle> viewLayout, WindowInformation windowInformation)
	{
		Items = items;
		ViewLayout = viewLayout;
		ParentWindowInformation = windowInformation;
	}

	public abstract void Close();

	public virtual bool ChangeLayout(List<ViewControlItem> items, List<Rectangle> viewLayout)
	{
		throw new NotImplementedException();
	}

	public void RaiseItemDataChanged(ViewControlItem viewControlItem, string data)
	{
		this.ItemDataChanged?.Invoke(this, new ViewControlItemDataArgs
		{
			ViewControlItem = viewControlItem,
			Data = data
		});
	}
}

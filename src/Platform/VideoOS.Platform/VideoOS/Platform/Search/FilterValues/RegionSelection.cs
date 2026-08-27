using System;
using System.Drawing;

namespace VideoOS.Platform.Search.FilterValues;

public class RegionSelection
{
	private string _mask;

	public Item Item { get; set; }

	public Size Size { get; set; }

	public string Mask
	{
		get
		{
			return _mask;
		}
		set
		{
			if (!object.Equals(_mask, value))
			{
				_mask = value;
				FireChangedEvent();
			}
		}
	}

	public event EventHandler Changed;

	private void FireChangedEvent()
	{
		this.Changed?.Invoke(this, EventArgs.Empty);
	}
}

using System;
using System.Drawing;

namespace VideoOS.Platform.Admin;

public class ContextMenuItem
{
	public virtual string DisplayName { get; set; }

	public virtual Image Icon { get; set; }

	public event EventHandler OnClickEvent;

	public virtual bool IsContextMenuValid()
	{
		return true;
	}

	public void FireOnClickEvent()
	{
		if (this.OnClickEvent != null)
		{
			this.OnClickEvent(this, new EventArgs());
		}
	}
}

using System;

namespace VideoOS.Common.Drivers;

[Serializable]
public class PTZRelativeArgs : PTZArgs
{
	public PTZRelativeArgs(Array names, Array values)
		: base(names, values)
	{
	}

	public PTZRelativeArgs(Array names, Array values, Array speedvalues)
		: base(names, values, speedvalues)
	{
	}

	public PTZRelativeArgs()
	{
	}

	public void MoveUp()
	{
		base.Tilt = (base.isTiltAvailable ? (base.Tilt - 1.0) : (-1.0));
	}

	public void MoveDown()
	{
		base.Tilt = (base.isTiltAvailable ? (base.Tilt + 1.0) : 1.0);
	}

	public void MoveLeft()
	{
		base.Pan = (base.isPanAvailable ? (base.Pan - 1.0) : (-1.0));
	}

	public void MoveRight()
	{
		base.Pan = (base.isPanAvailable ? (base.Pan + 1.0) : 1.0);
	}

	public void MoveUpLeft()
	{
		MoveUp();
		MoveLeft();
	}

	public void MoveUpRight()
	{
		MoveUp();
		MoveRight();
	}

	public void MoveDownLeft()
	{
		MoveDown();
		MoveLeft();
	}

	public void MoveDownRight()
	{
		MoveDown();
		MoveRight();
	}

	public void MoveZoomIn()
	{
		base.Zoom = (base.isZoomAvailable ? (base.Zoom + 1.0) : 1.0);
	}

	public void MoveZoomOut()
	{
		base.Zoom = (base.isZoomAvailable ? (base.Zoom - 1.0) : (-1.0));
	}
}

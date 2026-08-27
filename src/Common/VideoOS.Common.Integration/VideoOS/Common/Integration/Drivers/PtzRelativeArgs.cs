using System;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class PtzRelativeArgs : PtzArgs
{
	public void MoveUp()
	{
		base.Tilt = (base.TiltAvailable ? (base.Tilt - 1.0) : (-1.0));
	}

	public void MoveDown()
	{
		base.Tilt = (base.TiltAvailable ? (base.Tilt + 1.0) : 1.0);
	}

	public void MoveLeft()
	{
		base.Pan = (base.PanAvailable ? (base.Pan - 1.0) : (-1.0));
	}

	public void MoveRight()
	{
		base.Pan = (base.PanAvailable ? (base.Pan + 1.0) : 1.0);
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
		base.Zoom = (base.ZoomAvailable ? (base.Zoom + 1.0) : 1.0);
	}

	public void MoveZoomOut()
	{
		base.Zoom = (base.ZoomAvailable ? (base.Zoom - 1.0) : (-1.0));
	}
}

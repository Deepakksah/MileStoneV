using System;
using System.Drawing;

namespace VideoOS.Common.SharedComponent.Data.ViewLayout;

[Serializable]
public class ViewItem
{
	private Point _position;

	private Size _size;

	public Point Position
	{
		get
		{
			return _position;
		}
		set
		{
			_position = value;
		}
	}

	public Size Size
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}
}

using System.Drawing;

namespace VideoOS.Common.SharedComponent;

public class SizeAndLocation
{
	private Point _location;

	private Size _size;

	private int _positionNumber;

	public Point Location
	{
		get
		{
			return _location;
		}
		set
		{
			_location = value;
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

	public int PositionNumber
	{
		get
		{
			return _positionNumber;
		}
		set
		{
			_positionNumber = value;
		}
	}

	public SizeAndLocation(Size size, Point location, int positionNumber)
	{
		_size = size;
		_location = location;
		_positionNumber = positionNumber;
	}
}

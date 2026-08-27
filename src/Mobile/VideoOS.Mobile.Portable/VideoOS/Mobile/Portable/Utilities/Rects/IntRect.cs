using System;

namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class IntRect : Rectangle<int>, IEquatable<IntRect>
{
	public IntRect()
		: base((IOperators<int>)new IntOperators())
	{
	}

	public IntRect(int left, int top, int right, int bottom)
		: base((IOperators<int>)new IntOperators(), left, top, right, bottom)
	{
	}

	public IntRect(int x, int y, int width, int height, int dummy)
		: base((IOperators<int>)new IntOperators(), x, y, width, height, dummy)
	{
	}

	public bool ValidateRectParams()
	{
		if (base.Width < 0 || base.Height < 0 || base.X < 0 || base.Y < 0 || base.Top < 0 || base.Left < 0 || base.Right < 0 || base.Bottom < 0)
		{
			return false;
		}
		return true;
	}

	public bool Equals(IntRect other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (base.Left == other.Left && base.Right == other.Right && base.Top == other.Top)
		{
			return base.Bottom == other.Bottom;
		}
		return false;
	}

	public static bool operator ==(IntRect first, IntRect second)
	{
		if ((object)first == null)
		{
			return false;
		}
		if ((object)second == null)
		{
			return false;
		}
		if ((object)first == second)
		{
			return true;
		}
		return first.Equals(second);
	}

	public static bool operator !=(IntRect first, IntRect second)
	{
		return !(first == second);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as IntRect);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}

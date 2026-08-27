namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class Rectangle<T>
{
	private readonly IOperators<T> _operators;

	public T Left { get; set; }

	public T Top { get; set; }

	public T Right { get; set; }

	public T Bottom { get; set; }

	public T X
	{
		get
		{
			return Left;
		}
		set
		{
			Left = value;
		}
	}

	public T Y
	{
		get
		{
			return Top;
		}
		set
		{
			Top = value;
		}
	}

	public T Width
	{
		get
		{
			return _operators.Sub(Right, Left);
		}
		set
		{
			Right = _operators.Add(Left, value);
		}
	}

	public T Height
	{
		get
		{
			return _operators.Sub(Bottom, Top);
		}
		set
		{
			Bottom = _operators.Add(Top, value);
		}
	}

	public Rectangle(IOperators<T> operators)
	{
		_operators = operators;
		Left = (Top = (Right = (Bottom = default(T))));
	}

	public Rectangle(IOperators<T> operators, T left, T top, T right, T bottom)
	{
		_operators = operators;
		Left = left;
		Right = right;
		Top = top;
		Bottom = bottom;
		CorrectParams();
	}

	public Rectangle(IOperators<T> operators, T x, T y, T width, T height, int dummy)
	{
		_operators = operators;
		Left = x;
		Top = y;
		Width = width;
		Height = height;
		CorrectParams();
	}

	public Rectangle(Rectangle<T> other)
	{
		_operators = other._operators;
		Left = other.Left;
		Right = other.Right;
		Top = other.Top;
		Bottom = other.Bottom;
	}

	public void MoveX(T offset)
	{
		Left = _operators.Add(Left, offset);
		Right = _operators.Add(Right, offset);
	}

	public void MoveY(T offset)
	{
		Top = _operators.Add(Top, offset);
		Bottom = _operators.Add(Bottom, offset);
	}

	public virtual void CorrectParams()
	{
		if (_operators.Greater(Left, Right))
		{
			T left = Left;
			Left = Right;
			Right = left;
		}
		if (_operators.Greater(Top, Bottom))
		{
			T top = Top;
			Top = Bottom;
			Bottom = top;
		}
	}
}

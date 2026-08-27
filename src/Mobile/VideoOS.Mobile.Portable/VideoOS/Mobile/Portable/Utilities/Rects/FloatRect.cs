namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class FloatRect : Rectangle<double>
{
	public FloatRect()
		: base((IOperators<double>)new FloatOperators())
	{
	}

	public FloatRect(double left, double top, double right, double bottom)
		: base((IOperators<double>)new FloatOperators(), left, top, right, bottom)
	{
	}

	public FloatRect(double x, double y, double width, double height, int dummy)
		: base((IOperators<double>)new FloatOperators(), x, y, width, height, dummy)
	{
	}

	public FloatRect(FloatRect other)
		: base((Rectangle<double>)other)
	{
	}

	public override void CorrectParams()
	{
		if (base.Left < 0.0 || base.Left > 1.0)
		{
			base.Left = 0.0;
		}
		if (base.Right < 0.0 || base.Right > 1.0)
		{
			base.Right = 1.0;
		}
		if (base.Top < 0.0 || base.Top > 1.0)
		{
			base.Top = 0.0;
		}
		if (base.Bottom < 0.0 || base.Bottom > 1.0)
		{
			base.Bottom = 1.0;
		}
		base.CorrectParams();
	}
}

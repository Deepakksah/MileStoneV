namespace VideoOS.Mobile.Portable.Utilities.Rects;

public class FloatRectExt : FloatRect
{
	public FloatRectExt()
	{
	}

	public FloatRectExt(double left, double top, double right, double bottom)
		: base(left, top, right, bottom)
	{
	}

	public FloatRectExt(double x, double y, double width, double height, int dummy)
		: base(x, y, width, height, dummy)
	{
	}

	public FloatRectExt(FloatRectExt other)
		: base(other)
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

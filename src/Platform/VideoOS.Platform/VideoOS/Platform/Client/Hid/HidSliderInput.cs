using System;

namespace VideoOS.Platform.Client.Hid;

public sealed class HidSliderInput : HidInput
{
	public static readonly Guid ActionZoom = new Guid("D38C50E9-0DE2-47fc-99EC-F81A8A2B3C3A");

	private const double _TOLERANCE = 1E-05;

	private double _value;

	public double Value
	{
		get
		{
			return _value;
		}
		set
		{
			if (!(Math.Abs(_value - value) < 1E-05))
			{
				if (value < 0.0 || value > 1.0)
				{
					throw new ArgumentMIPException("Argument is out of range: [0.0 .. 1.0].");
				}
				_value = value;
				FireValueChangedEvent(new HidSliderValueChangedEventArgs(this, value));
			}
		}
	}

	public HidSliderInput(Guid id, string name, Guid defaultActionId)
		: base(id, name, defaultActionId)
	{
	}

	public HidSliderInput(Guid id, string name)
		: this(id, name, HidInput.ActionNoAction)
	{
	}
}

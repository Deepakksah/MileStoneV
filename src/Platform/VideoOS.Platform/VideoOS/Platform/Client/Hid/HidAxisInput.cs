using System;

namespace VideoOS.Platform.Client.Hid;

public sealed class HidAxisInput : HidInput
{
	public static readonly Guid ActionPan = new Guid("5F349168-B931-4cba-8EC4-A568CEC7A104");

	public static readonly Guid ActionTilt = new Guid("44991F59-9E4B-4a74-B20A-BF4A8C4ACE87");

	public static readonly Guid ActionZoom = new Guid("D38C50E9-0DE2-47fc-99EC-F81A8A2B3C3A");

	public static readonly Guid ActionScrub = new Guid("BD858F24-C6EB-4263-BCF3-8B163B3E7381");

	private const double _TOLERANCE = 1E-05;

	private double _value;

	private bool _defaultInvert;

	private bool _defaultAbsolute;

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
				if (value < -1.0 || value > 1.0)
				{
					throw new ArgumentMIPException("Argument is out of range: [-1.0 .. 1.0].");
				}
				_value = value;
				FireValueChangedEvent(new HidAxisValueChangedEventArgs(this, value));
			}
		}
	}

	public bool DefaultInvert => _defaultInvert;

	public bool DefaultAbsolute => _defaultAbsolute;

	public HidAxisInput(Guid id, string name, Guid defaultActionId = default(Guid), bool defaultInvert = false, bool defaultAbsolute = false)
		: base(id, name, (defaultActionId == default(Guid)) ? HidInput.ActionNoAction : defaultActionId)
	{
		_defaultInvert = defaultInvert;
		_defaultAbsolute = defaultAbsolute;
	}

	public HidAxisInput(Guid id, string name, Guid defaultActionId)
		: base(id, name, defaultActionId)
	{
	}

	public HidAxisInput(Guid id, string name)
		: this(id, name, HidInput.ActionNoAction)
	{
	}
}

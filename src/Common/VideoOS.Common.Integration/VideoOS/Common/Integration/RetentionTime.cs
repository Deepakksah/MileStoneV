namespace VideoOS.Common.Integration;

public class RetentionTime
{
	private int _unit;

	private OptionType _option;

	public int Unit
	{
		get
		{
			return _unit;
		}
		set
		{
			_unit = value;
		}
	}

	public OptionType Option
	{
		get
		{
			return _option;
		}
		set
		{
			_option = value;
		}
	}
}

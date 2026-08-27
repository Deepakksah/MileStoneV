namespace VideoOS.Utilities.Service;

public class FailureAction
{
	private RecoverAction type;

	private int delay;

	public RecoverAction Type
	{
		get
		{
			return type;
		}
		set
		{
			type = value;
		}
	}

	public int Delay
	{
		get
		{
			return delay;
		}
		set
		{
			delay = value;
		}
	}

	public FailureAction()
	{
	}

	public FailureAction(RecoverAction actionType, int actionDelay)
	{
		type = actionType;
		delay = actionDelay;
	}
}

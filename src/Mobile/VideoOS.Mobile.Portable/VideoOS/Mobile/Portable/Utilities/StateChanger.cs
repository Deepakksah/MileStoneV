using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public class StateChanger<T> : IStateChange<T>, IStateChangeInternal<T> where T : IComparable
{
	public T State { get; private set; }

	public event StateEventHandler<T> OnStateChange;

	public StateChanger(T initialState = default(T))
	{
		State = initialState;
	}

	public StateEventArgs<T> GetState()
	{
		return new StateEventArgs<T>(State);
	}

	public void ChangeState(T state)
	{
		try
		{
			if (!State.Equals(state))
			{
				State = state;
				if (this.OnStateChange != null)
				{
					this.OnStateChange(this, GetState());
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "StateChanger.ChangeState", ex.Message);
		}
	}
}

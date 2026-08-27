using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class StateChanger : IStateChange, IStateChangeInternal
{
	public ConnectionStates State { get; private set; }

	public event EventHandler OnStateChange;

	public StateChanger(ConnectionStates initialState = ConnectionStates.Disconnected)
	{
		State = initialState;
	}

	public EventArgs GetState()
	{
		return GetConnectionEventArgs();
	}

	public ConnectionEventArgs GetConnectionEventArgs()
	{
		return new ConnectionEventArgs(State);
	}

	public void ChangeState(ConnectionStates state)
	{
		try
		{
			if (State != state)
			{
				State = state;
				if (this.OnStateChange != null)
				{
					this.OnStateChange(this, GetConnectionEventArgs());
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "Connection.ChangeState", ex.Message);
		}
	}
}

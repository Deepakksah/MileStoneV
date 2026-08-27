using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACStateManager
{
	public event EventHandler<ACStatesChangedEventArgs> StatesChanged = delegate
	{
	};

	public event EventHandler<ACStatesInvalidatedEventArgs> StatesInvalidated = delegate
	{
	};

	public abstract IEnumerable<ACState> GetStates(IEnumerable<string> operationableInstanceIds);

	protected void FireStatesChanged(IEnumerable<ACState> acStates)
	{
		this.StatesChanged(this, new ACStatesChangedEventArgs(acStates));
	}

	protected void FireStatesInvalidated(IEnumerable<string> operationableInstanceIds)
	{
		this.StatesInvalidated(this, new ACStatesInvalidatedEventArgs(operationableInstanceIds));
	}
}

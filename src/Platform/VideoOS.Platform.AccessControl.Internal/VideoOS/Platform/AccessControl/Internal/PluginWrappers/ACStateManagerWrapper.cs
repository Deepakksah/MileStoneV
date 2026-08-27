using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACStateManagerWrapper
{
	private ACStateManager _acStateManager;

	public event EventHandler<ACStatesChangedEventArgs> StatesChanged = delegate
	{
	};

	public event EventHandler<ACStatesInvalidatedEventArgs> StatesInvalidated = delegate
	{
	};

	public ACStateManagerWrapper(ACStateManager acStateManager)
	{
		_acStateManager = acStateManager;
		_acStateManager.StatesChanged += _acStateManager_StatesChanged;
		_acStateManager.StatesInvalidated += _acStateManager_StatesInvalidated;
	}

	public IEnumerable<ACState> GetStates(IEnumerable<string> operationableInstanceIds)
	{
		operationableInstanceIds = new ReadOnlyCollection<string>((operationableInstanceIds != null) ? operationableInstanceIds.ToList() : new List<string>());
		IEnumerable<ACState> enumerable = ACPluginWrapper.TryCallPlugin(() => _acStateManager.GetStates(operationableInstanceIds), _acStateManager.GetType(), "GetStates()", null);
		return new ReadOnlyCollection<ACState>((enumerable != null) ? (from s in enumerable
			group s by s.OperationableInstanceId into g
			select g.First()).ToList() : new List<ACState>());
	}

	private void _acStateManager_StatesChanged(object sender, ACStatesChangedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.StatesChanged(this, e);
		}, _acStateManager.GetType(), "StatesChanged");
	}

	private void _acStateManager_StatesInvalidated(object sender, ACStatesInvalidatedEventArgs e)
	{
		ACPluginWrapper.TryHandleEvent(delegate
		{
			this.StatesInvalidated(this, e);
		}, _acStateManager.GetType(), "StatesInvalidated");
	}
}

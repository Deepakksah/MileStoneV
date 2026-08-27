using System;
using System.Collections.ObjectModel;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.RuleAction;

public abstract class ActionManager
{
	public event EventHandler ActionConfigurationChanged = delegate
	{
	};

	public virtual void ExecuteAction2(Guid actionId, Collection<FQID> actionItems, Collection<FQID> actionItems2, BaseEvent sourceEvent)
	{
		if (actionItems2 != null)
		{
			EnvironmentManager.Instance.Log(error: false, "ActionManager", "Paramaters for action2 found, but ExecuteAction2 not implemented");
		}
		ExecuteAction(actionId, actionItems, sourceEvent);
	}

	public abstract void ExecuteAction(Guid actionId, Collection<FQID> actionItems, BaseEvent sourceEvent);

	public abstract Collection<ActionDefinition> GetActionDefinitions();

	protected virtual void OnActionConfigurationChanged(EventArgs e)
	{
		this.ActionConfigurationChanged?.Invoke(this, e);
	}
}

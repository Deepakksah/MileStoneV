using System;

namespace VideoOS.Utilities.Workflow;

public abstract class CompareItemsUpdateStrategy
{
	protected IDecisionItem[] _decisionItems;

	private bool _forceUpdate;

	public T GetValue<T>(int index)
	{
		ValidateDecisionItems();
		return (T)_decisionItems[index].GetNewestValue();
	}

	public bool IsUpdateNeeded()
	{
		ValidateDecisionItems();
		int num = 0;
		bool flag = _forceUpdate;
		while (num < _decisionItems.Length)
		{
			IDecisionItem decisionItem = _decisionItems[num++];
			decisionItem.CalculateNewValue();
			if (flag || decisionItem.IsDifferent())
			{
				decisionItem.SaveNewValue();
				flag = true;
			}
		}
		_forceUpdate = false;
		return flag;
	}

	public void ForceUpdate()
	{
		_forceUpdate = true;
	}

	private void ValidateDecisionItems()
	{
		if (_decisionItems == null)
		{
			throw new InvalidOperationException("Decision items not initialized.");
		}
	}
}

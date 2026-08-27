using System;
using System.Globalization;
using VideoOS.Utilities.Validation;

namespace VideoOS.Utilities.Workflow;

public class InitControl : IInitControl
{
	private bool _isInit;

	private readonly object _initLock = new object();

	private readonly object _owner;

	public InitControl(object owner)
	{
		Checks.IsNull(owner, "owner", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Utilities\\VideoOS.Utilities\\Workflow\\InitControl.cs", 24);
		_owner = owner;
	}

	public void Init(Action continueAction)
	{
		Init(continueAction, null);
	}

	public void Close(Action continueAction)
	{
		Close(continueAction, null);
	}

	public void Init(Action continueAction, Action<string> refuseAction)
	{
		TryUpdateStateAndContinue(expectedCurrentState: false, continueAction, refuseAction);
	}

	public void Close(Action continueAction, Action<string> refuseAction)
	{
		TryUpdateStateAndContinue(expectedCurrentState: true, continueAction, refuseAction);
	}

	private void TryUpdateStateAndContinue(bool expectedCurrentState, Action continueAction, Action<string> refuseAction)
	{
		Checks.IsNull(continueAction, "continueAction", "TryUpdateStateAndContinue", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.Utilities\\VideoOS.Utilities\\Workflow\\InitControl.cs", 68);
		lock (_initLock)
		{
			if (_isInit != expectedCurrentState)
			{
				refuseAction?.Invoke(string.Format(CultureInfo.InvariantCulture, "The object {0} is expected to be {1}", _owner, GetExpectationMessage(expectedCurrentState)));
				return;
			}
			_isInit = !expectedCurrentState;
		}
		continueAction();
	}

	private static string GetExpectationMessage(bool state)
	{
		if (!state)
		{
			return "not initialized when doing initialization";
		}
		return "already initialized when doing deinitialization";
	}
}

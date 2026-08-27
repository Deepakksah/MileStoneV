using System;

namespace VideoOS.Utilities.Workflow;

public interface IInitControl
{
	void Init(Action continueAction);

	void Init(Action continueAction, Action<string> refuseAction);

	void Close(Action continueAction);

	void Close(Action continueAction, Action<string> refuseAction);
}

using System;

namespace VideoOS.Mobile.Portable.Utilities.Async;

public class ActionWrapper<T>
{
	private readonly Action<T> _action;

	private readonly T _object;

	public ActionWrapper(Action<T> action, T tObject)
	{
		_action = action;
		_object = tObject;
	}

	public void DoTheAction()
	{
		_action(_object);
	}
}

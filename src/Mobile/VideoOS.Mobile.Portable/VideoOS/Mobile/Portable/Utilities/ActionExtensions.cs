using System;

namespace VideoOS.Mobile.Portable.Utilities;

public static class ActionExtensions
{
	public static Func<TaskFuncResult> ToFunOfTaskFuncResult(this Action action)
	{
		return delegate
		{
			action();
			return TaskFuncResult.SleepForaWhile;
		};
	}
}

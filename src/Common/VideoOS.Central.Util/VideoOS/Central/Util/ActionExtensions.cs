using System;

namespace VideoOS.Central.Util;

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

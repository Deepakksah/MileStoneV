using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public static class ExceptionHelper
{
	public static void CallCatched(Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ExceptionHelper.CallCatched()", ex.Message);
		}
	}

	public static void CallCatched<T>(Action<T> action, T param)
	{
		CallCatched(delegate
		{
			action(param);
		});
	}
}

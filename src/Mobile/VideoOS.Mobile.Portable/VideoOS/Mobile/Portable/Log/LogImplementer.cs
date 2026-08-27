using System;

namespace VideoOS.Mobile.Portable.Log;

public class LogImplementer : ILog
{
	public static ILog Instance = new LogImplementer();

	private LogImplementer()
	{
	}

	public void Log(LogContent content, string origin, string text, object dump = null)
	{
		switch (content)
		{
		case LogContent.Disabled:
			VideoOS.Mobile.Portable.Log.Log.Internal(origin, text);
			break;
		case LogContent.Error:
			VideoOS.Mobile.Portable.Log.Log.Error(origin, text);
			break;
		case LogContent.Warning:
			VideoOS.Mobile.Portable.Log.Log.Warning(origin, text);
			break;
		case LogContent.Info:
			VideoOS.Mobile.Portable.Log.Log.Info(origin, text);
			break;
		case LogContent.Debug:
			VideoOS.Mobile.Portable.Log.Log.Debug(origin, text);
			break;
		case LogContent.Trace:
			VideoOS.Mobile.Portable.Log.Log.Trace(origin, text);
			break;
		case LogContent.Communication:
			VideoOS.Mobile.Portable.Log.Log.Communication(origin, text);
			break;
		default:
			throw new ArgumentOutOfRangeException("content");
		}
	}
}

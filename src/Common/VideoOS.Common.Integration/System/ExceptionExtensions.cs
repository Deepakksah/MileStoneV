using System.Reflection;

namespace System;

public static class ExceptionExtensions
{
	private static readonly MethodInfo _prepForRemotingMethodInfo = GetPrepForRemotingMethodInfo();

	private static readonly object[] _emptyParams = new object[0];

	private static MethodInfo GetPrepForRemotingMethodInfo()
	{
		try
		{
			return typeof(Exception).GetMethod("PrepForRemoting", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		catch
		{
			return null;
		}
	}

	public static Exception PrepareForRethrow(this Exception exception)
	{
		if (_prepForRemotingMethodInfo != null)
		{
			try
			{
				_prepForRemotingMethodInfo.Invoke(exception, _emptyParams);
			}
			catch
			{
			}
		}
		return exception;
	}
}

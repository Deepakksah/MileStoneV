using System;
using System.Reflection;

namespace VideoOS.Mobile.Portable.Utilities;

public class GetHresultException
{
	private readonly Exception _base;

	public Exception Inner => _base;

	public int GetHresult
	{
		get
		{
			int result = 0;
			try
			{
				PropertyInfo runtimeProperty = typeof(Exception).GetRuntimeProperty("HResult");
				if (runtimeProperty.GetMethod.IsStatic && runtimeProperty.GetMethod.IsPrivate)
				{
					result = (int)runtimeProperty.GetValue(_base, null);
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}

	public GetHresultException(Exception exception)
	{
		_base = exception;
	}
}

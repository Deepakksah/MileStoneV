using System;
using System.Linq;
using System.Reflection;

namespace VideoOS.Mobile.Utilities;

public static class DynamicMethodInvoker
{
	public static T InvokeStaticMethod<T>(Type staticInstanceType, string methodName, (Type, object)[] parameters, string fallbackMethodName = null, (Type, object)[] fallbackMethodParams = null)
	{
		if (staticInstanceType == null)
		{
			throw new ArgumentNullException("staticInstanceType");
		}
		if (string.IsNullOrWhiteSpace(methodName))
		{
			throw new ArgumentException("methodName parameter must be specified");
		}
		if (TryInvokeStaticMethodInternal<T>(staticInstanceType, methodName, parameters, out var result))
		{
			return result;
		}
		if (!string.IsNullOrWhiteSpace(fallbackMethodName))
		{
			if (TryInvokeStaticMethodInternal<T>(staticInstanceType, fallbackMethodName, fallbackMethodParams, out var result2))
			{
				return result2;
			}
			throw new MissingMethodException(typeof(T).FullName, methodName + " and " + fallbackMethodName + " does not exists.");
		}
		throw new MissingMethodException(typeof(T).FullName, methodName);
	}

	private static bool TryInvokeStaticMethodInternal<T>(Type staticInstanceType, string methodName, (Type, object)[] @params, out T result)
	{
		result = default(T);
		Type[] array = @params?.Select(((Type, object) p) => p.Item1)?.ToArray();
		object[] parameters = @params?.Select(((Type, object) p) => p.Item2)?.ToArray();
		MethodInfo method = staticInstanceType.GetMethod(methodName, array ?? Type.EmptyTypes);
		if (method != null)
		{
			result = (T)method.Invoke(null, parameters);
			return true;
		}
		return false;
	}
}

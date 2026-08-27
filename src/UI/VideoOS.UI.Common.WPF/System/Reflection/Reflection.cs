using System.Linq.Expressions;

namespace System.Reflection;

public static class Reflection
{
	public static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TProperty>> propertyExpression)
	{
		return (propertyExpression.Body as MemberExpression).Member as PropertyInfo;
	}

	public static MethodInfo GetMethodInfo(Expression<Action> methodExpression)
	{
		return (methodExpression.Body as MethodCallExpression).Method;
	}
}
public static class Reflection<T>
{
	public static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
	{
		return (propertyExpression.Body as MemberExpression).Member as PropertyInfo;
	}

	public static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<TProperty>> propertyExpression)
	{
		return (propertyExpression.Body as MemberExpression).Member as PropertyInfo;
	}

	public static MethodInfo GetMethodInfo<TMethod>(Expression<Func<T, TMethod>> methodExpression)
	{
		return (methodExpression.Body as MethodCallExpression).Method;
	}

	public static MethodInfo GetMethodInfo(Expression<Action<T>> methodExpression)
	{
		return (methodExpression.Body as MethodCallExpression).Method;
	}
}

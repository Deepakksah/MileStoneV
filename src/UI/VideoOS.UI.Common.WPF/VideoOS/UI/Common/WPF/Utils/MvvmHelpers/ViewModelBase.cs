using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public class ViewModelBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected bool SetProperty<TProperty>(ref TProperty storageField, TProperty value, Expression<Func<TProperty>> property, string propertyName)
	{
		if (object.Equals(storageField, value))
		{
			return false;
		}
		storageField = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	protected bool SetProperty<TProperty>(ref TProperty storageField, TProperty value, [CallerMemberName] string propertyName = "")
	{
		if (object.Equals(storageField, value))
		{
			return false;
		}
		storageField = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	protected bool SetProperty<TProperty>(Func<TProperty> getPropertyValue, Action<TProperty> setPropertyValue, TProperty value, [CallerMemberName] string propertyName = "")
	{
		if (object.Equals(getPropertyValue(), value))
		{
			return false;
		}
		setPropertyValue(value);
		OnPropertyChanged(propertyName);
		return true;
	}

	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private static MemberInfo GetMemberInfo(Expression expression)
	{
		LambdaExpression lambdaExpression = (LambdaExpression)expression;
		MemberExpression memberExpression = ((!(lambdaExpression.Body is UnaryExpression)) ? ((MemberExpression)lambdaExpression.Body) : ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand));
		return memberExpression.Member;
	}

	public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
	{
		return (propertyExpression.Body as MemberExpression).Member.Name;
	}
}

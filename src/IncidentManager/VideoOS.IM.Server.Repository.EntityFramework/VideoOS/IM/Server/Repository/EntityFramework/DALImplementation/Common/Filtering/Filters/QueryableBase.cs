using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering.Filters;

public abstract class QueryableBase<TDbObject> where TDbObject : ARepoBaseEntity
{
	protected ParameterExpression ParameterExpression => Expression.Parameter(typeof(TDbObject), "p");

	public abstract Expression<Func<TDbObject, bool>> GetFilteringExpression(string propertyName, string val);

	protected MemberExpression GetMemberExpression(string propertyName, ParameterExpression parameterExpression)
	{
		return Expression.Property(parameterExpression, propertyName);
	}

	protected UnaryExpression GetValueExpression(string propertyName, string val, ParameterExpression param, bool inForceString = false)
	{
		Type type = ((PropertyInfo)Expression.Property(param, propertyName).Member).PropertyType;
		if (inForceString)
		{
			type = typeof(string);
		}
		TypeConverter converter = TypeDescriptor.GetConverter(type);
		if (!converter.CanConvertFrom(typeof(string)))
		{
			throw new NotSupportedException();
		}
		return Expression.Convert(Expression.Constant(converter.ConvertFromInvariantString(val)), type);
	}
}

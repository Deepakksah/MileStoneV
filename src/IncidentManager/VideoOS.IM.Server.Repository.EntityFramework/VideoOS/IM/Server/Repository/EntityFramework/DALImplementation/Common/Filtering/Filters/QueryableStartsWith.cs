using System;
using System.Linq.Expressions;
using System.Reflection;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering.Filters;

public sealed class QueryableStartsWith<TDbObject> : QueryableBase<TDbObject> where TDbObject : ARepoBaseEntity
{
	public override Expression<Func<TDbObject, bool>> GetFilteringExpression(string propertyName, string val)
	{
		ParameterExpression parameterExpression = base.ParameterExpression;
		MemberExpression memberExpression = GetMemberExpression(propertyName, parameterExpression);
		UnaryExpression valueExpression = GetValueExpression(propertyName, val, parameterExpression, inForceString: true);
		MethodInfo method = typeof(string).GetMethod("StartsWith", new Type[1] { typeof(string) });
		MethodInfo method2 = typeof(object).GetMethod("ToString");
		return Expression.Lambda<Func<TDbObject, bool>>(Expression.Call(Expression.Call(memberExpression, method2), method, valueExpression), new ParameterExpression[1] { parameterExpression });
	}
}

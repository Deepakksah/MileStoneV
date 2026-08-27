using System;
using System.Linq.Expressions;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering.Filters;

public sealed class QueryableEqual<TDbObject> : QueryableBase<TDbObject> where TDbObject : ARepoBaseEntity
{
	public override Expression<Func<TDbObject, bool>> GetFilteringExpression(string propertyName, string val)
	{
		ParameterExpression parameterExpression = base.ParameterExpression;
		MemberExpression memberExpression = GetMemberExpression(propertyName, parameterExpression);
		UnaryExpression valueExpression = GetValueExpression(propertyName, val, parameterExpression);
		return Expression.Lambda<Func<TDbObject, bool>>(Expression.Equal(memberExpression, valueExpression), new ParameterExpression[1] { parameterExpression });
	}
}

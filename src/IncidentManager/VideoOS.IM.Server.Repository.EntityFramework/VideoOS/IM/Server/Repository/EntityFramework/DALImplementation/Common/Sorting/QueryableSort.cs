using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Sorting;

public sealed class QueryableSort<TDbObject> where TDbObject : ARepoBaseEntity
{
	public IQueryable<TDbObject> GetDynamicQueryWithExpressionTrees(IQueryable<TDbObject> source, string propertyName, ListSortDirection inSortOrder)
	{
		Type typeFromHandle = typeof(TDbObject);
		ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "p");
		MemberExpression memberExpression = Expression.Property(parameterExpression, propertyName);
		LambdaExpression expression = Expression.Lambda(Expression.MakeMemberAccess(parameterExpression, memberExpression.Member), parameterExpression);
		Type[] typeArguments = new Type[2] { typeFromHandle, memberExpression.Type };
		string methodName = ((inSortOrder == ListSortDirection.Ascending) ? "OrderBy" : "OrderByDescending");
		MethodCallExpression expression2 = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(expression));
		return source.Provider.CreateQuery<TDbObject>(expression2);
	}
}

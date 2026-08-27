using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace VideoOS.IdentityServer.Extensions;

public static class IQueryAbleExtensions
{
	public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
	{
		if (includes != null)
		{
			query = includes.Aggregate(query, (IQueryable<T> current, Expression<Func<T, object>> include) => current.Include(include));
		}
		return query;
	}

	public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params string[] includes) where T : class
	{
		if (includes != null)
		{
			query = includes.Aggregate(query, (IQueryable<T> current, string include) => current.Include(include));
		}
		return query;
	}

	public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int skip, int take)
	{
		if (skip < 0)
		{
			throw new ArgumentOutOfRangeException("skip");
		}
		if (take < 0)
		{
			throw new ArgumentOutOfRangeException("take");
		}
		if (!(queryable is IOrderedQueryable<T>))
		{
			throw new ArgumentException("queryable", "Take/Skip returns inconsistent results if not oredered. Call OrderBy() before calling Paginate.");
		}
		return queryable.Skip(skip).Take(take);
	}
}

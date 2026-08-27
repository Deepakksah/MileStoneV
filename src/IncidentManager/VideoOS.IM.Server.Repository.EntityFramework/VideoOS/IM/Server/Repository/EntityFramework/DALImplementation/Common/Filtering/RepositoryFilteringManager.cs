using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Filtering;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Sorting;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering.Filters;
using VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Sorting;
using VideoOS.IM.Server.RepositoryModel.Model.Abstractions.Common;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Filters;

namespace VideoOS.IM.Server.Repository.EntityFramework.DALImplementation.Common.Filtering;

public sealed class RepositoryFilteringManager<TDbObject> where TDbObject : ARepoBaseEntity
{
	private static readonly Dictionary<EFilterTypes, QueryableBase<TDbObject>> FilterExpressions = new Dictionary<EFilterTypes, QueryableBase<TDbObject>>
	{
		{
			EFilterTypes.co,
			new QueryableContains<TDbObject>()
		},
		{
			EFilterTypes.ew,
			new QueryableEndsWith<TDbObject>()
		},
		{
			EFilterTypes.eq,
			new QueryableEqual<TDbObject>()
		},
		{
			EFilterTypes.sw,
			new QueryableStartsWith<TDbObject>()
		},
		{
			EFilterTypes.neq,
			new QueryableNotEqual<TDbObject>()
		},
		{
			EFilterTypes.ltoe,
			new QueryableLessThanOrEqual<TDbObject>()
		},
		{
			EFilterTypes.gtoe,
			new QueryableGreaterThanOrEqual<TDbObject>()
		}
	};

	private static readonly QueryableSort<TDbObject> SortExpression = new QueryableSort<TDbObject>();

	public static IQueryable<TDbObject> ApplyGlobalGeneralPropertyFilter(ref IQueryable<TDbObject> inQueryable, RepositoryFilter itemF)
	{
		if (itemF == null)
		{
			return inQueryable;
		}
		if (itemF.ApplyFilter && FilterExpressions.TryGetValue(itemF.FilterType, out var value))
		{
			Expression<Func<TDbObject, bool>> filteringExpression = value.GetFilteringExpression(itemF.PropName, itemF.FilterValue);
			inQueryable = inQueryable.Where(filteringExpression);
		}
		if (itemF.ApplySort)
		{
			ListSortDirection inSortOrder = ((itemF.SortType != ESortTypes.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
			inQueryable = SortExpression.GetDynamicQueryWithExpressionTrees(inQueryable, itemF.PropName, inSortOrder);
		}
		return inQueryable;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Filtering;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Sorting;
using VideoOS.IM.Server.Logger;

namespace VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Filters;

public static class RepositoryFilterConverter
{
	public static EFilterTypes defaultFilterType = EFilterTypes.sw;

	public static ESortTypes defaultSortType = ESortTypes.Ascending;

	public static List<RepositoryFilter> ConvertToRepositoryFilters(Dictionary<string, string> filters, Dictionary<string, string> filtersType, Dictionary<string, string> orders)
	{
		List<RepositoryFilter> list = new List<RepositoryFilter>();
		try
		{
			if (filters != null)
			{
				foreach (KeyValuePair<string, string> filter in filters)
				{
					if (filter.Key != null && filter.Value != null)
					{
						list.AddOrUpdateFilter_ByFilterValue(filter.Key, filter.Value);
					}
				}
			}
			if (filtersType != null)
			{
				foreach (KeyValuePair<string, string> item in filtersType)
				{
					if (item.Key != null && item.Value != null && Enum.TryParse<EFilterTypes>(item.Value, ignoreCase: true, out var result))
					{
						list.AddOrUpdateFilter_ByFilterType(item.Key, result);
					}
				}
			}
			if (orders != null)
			{
				foreach (KeyValuePair<string, string> order in orders)
				{
					if (order.Key != null && order.Value != null && Enum.TryParse<ESortTypes>(order.Value, ignoreCase: true, out var result2))
					{
						list.AddOrUpdateFilter_ByFilterSort(order.Key, result2);
					}
				}
			}
		}
		catch (Exception arg)
		{
			LoggerProvider.Instance.Logger.LogError($"Error happened converting to repository filter: {arg}");
		}
		return list;
	}

	public static void AddOrUpdateFilter_ByFilterValue(this List<RepositoryFilter> inThis, string inPropName, string inFilterValue)
	{
		if (inThis == null)
		{
			inThis = new List<RepositoryFilter>();
		}
		RepositoryFilter repositoryFilter = inThis.FirstOrDefault((RepositoryFilter o) => o.PropName.Equals(inPropName, StringComparison.InvariantCultureIgnoreCase));
		if (repositoryFilter == null)
		{
			repositoryFilter = new RepositoryFilter
			{
				ApplyFilter = true,
				FilterType = defaultFilterType,
				FilterValue = inFilterValue,
				PropName = inPropName,
				ApplySort = false,
				SortType = defaultSortType
			};
			inThis.Add(repositoryFilter);
		}
		else
		{
			repositoryFilter.ApplyFilter = true;
			repositoryFilter.FilterValue = inFilterValue;
		}
	}

	public static void AddOrUpdateFilter_ByFilterType(this List<RepositoryFilter> inThis, string inPropName, EFilterTypes inFilterType)
	{
		if (inThis == null)
		{
			inThis = new List<RepositoryFilter>();
		}
		RepositoryFilter repositoryFilter = inThis.FirstOrDefault((RepositoryFilter o) => o.PropName.Equals(inPropName, StringComparison.InvariantCultureIgnoreCase));
		if (repositoryFilter == null)
		{
			repositoryFilter = new RepositoryFilter
			{
				ApplyFilter = false,
				FilterType = inFilterType,
				FilterValue = "",
				PropName = inPropName,
				ApplySort = false,
				SortType = defaultSortType
			};
			inThis.Add(repositoryFilter);
		}
		else
		{
			repositoryFilter.FilterType = inFilterType;
		}
	}

	public static void AddOrUpdateFilter_ByFilterSort(this List<RepositoryFilter> inThis, string inPropName, ESortTypes inFilterSortType)
	{
		if (inThis == null)
		{
			inThis = new List<RepositoryFilter>();
		}
		RepositoryFilter repositoryFilter = inThis.FirstOrDefault((RepositoryFilter o) => o.PropName.Equals(inPropName, StringComparison.InvariantCultureIgnoreCase));
		if (repositoryFilter == null)
		{
			repositoryFilter = new RepositoryFilter
			{
				ApplyFilter = false,
				FilterType = defaultFilterType,
				FilterValue = "",
				PropName = inPropName,
				ApplySort = true,
				SortType = inFilterSortType
			};
			inThis.Add(repositoryFilter);
		}
		else
		{
			repositoryFilter.SortType = inFilterSortType;
			repositoryFilter.ApplySort = true;
		}
	}
}

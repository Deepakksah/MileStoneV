using System.Collections.Generic;
using VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Filters;

namespace VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Paging;

public class RepositoryPaging
{
	private List<RepositoryFilter> repositoryFilters;

	public static int GlobalPageCountSize { get; set; } = 10000;

	public int Page { get; set; }

	public int CountPerPage { get; set; }

	public int TotalPages { get; set; }

	public int TotalCount { get; set; }

	public Dictionary<string, string> Filters { get; set; }

	public Dictionary<string, string> FiltersType { get; set; }

	public Dictionary<string, string> Orders { get; set; }

	public List<RepositoryFilter> RepositoryFilters
	{
		get
		{
			if (repositoryFilters == null)
			{
				repositoryFilters = RepositoryFilterConverter.ConvertToRepositoryFilters(Filters, FiltersType, Orders);
			}
			return repositoryFilters;
		}
		set
		{
			repositoryFilters = value;
		}
	}

	public RepositoryPaging()
	{
		Filters = new Dictionary<string, string>();
		FiltersType = new Dictionary<string, string>();
		Orders = new Dictionary<string, string>();
		CountPerPage = GlobalPageCountSize;
		Page = 1;
	}
}

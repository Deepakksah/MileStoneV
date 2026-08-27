using VideoOS.IM.Common.GlobalDefinitions.Enums.Filtering;
using VideoOS.IM.Common.GlobalDefinitions.Enums.Sorting;

namespace VideoOS.IM.Server.RepositoryModel.RepoRequestObjectModels.Filters;

public class RepositoryFilter
{
	public string PropName { get; set; }

	public string FilterValue { get; set; }

	public EFilterTypes FilterType { get; set; }

	public bool ApplyFilter { get; set; }

	public ESortTypes SortType { get; set; }

	public bool ApplySort { get; set; }
}

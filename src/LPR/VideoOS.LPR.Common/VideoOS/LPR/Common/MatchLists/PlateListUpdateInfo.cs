using System;

namespace VideoOS.LPR.Common.MatchLists;

public class PlateListUpdateInfo
{
	public int UpdateId { get; }

	public Guid MatchListId { get; }

	public int Page { get; }

	public int PageSize { get; }

	public string SearchFilter { get; }

	public MatchListItemViewModel[] MatchListItemsViewModels { get; set; }

	public PlateListUpdateInfo(int updateId, Guid matchListId, int page, int pageSize, string searchFilter)
	{
		UpdateId = updateId;
		MatchListId = matchListId;
		Page = page;
		PageSize = pageSize;
		SearchFilter = searchFilter;
	}
}

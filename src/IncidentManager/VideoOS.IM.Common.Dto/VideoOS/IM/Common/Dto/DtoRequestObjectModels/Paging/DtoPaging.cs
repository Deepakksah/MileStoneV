using System.Collections.Generic;

namespace VideoOS.IM.Common.Dto.DtoRequestObjectModels.Paging;

public class DtoPaging
{
	public int Page { get; set; }

	public int CountPerPage { get; set; }

	public int TotalPages { get; set; }

	public int TotalCount { get; set; }

	public Dictionary<string, string> Filters { get; set; }

	public Dictionary<string, string> FiltersType { get; set; }

	public Dictionary<string, string> Orders { get; set; }

	public DtoPaging()
	{
		Filters = new Dictionary<string, string>();
		FiltersType = new Dictionary<string, string>();
		Orders = new Dictionary<string, string>();
	}
}

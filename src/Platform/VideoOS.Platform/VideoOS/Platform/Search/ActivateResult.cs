using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Search;

public sealed class ActivateResult
{
	public static ActivateResult Default { get; } = new ActivateResult();

	public IEnumerable<SearchResultData> ModifiedResults { get; set; } = Enumerable.Empty<SearchResultData>();

	public IEnumerable<SearchResultData> DeletedResults { get; set; } = Enumerable.Empty<SearchResultData>();

	public bool RestartSearch { get; set; }
}

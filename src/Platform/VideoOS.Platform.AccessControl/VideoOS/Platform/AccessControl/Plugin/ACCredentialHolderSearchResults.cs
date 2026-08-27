using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCredentialHolderSearchResults
{
	public IEnumerable<ACCredentialHolderSearchResult> SearchResults { get; private set; }

	public bool PartialResult { get; private set; }

	public ACCredentialHolderSearchResults(IEnumerable<ACCredentialHolderSearchResult> searchResults, bool partialResult)
	{
		SearchResults = new ReadOnlyCollection<ACCredentialHolderSearchResult>((searchResults != null) ? searchResults.ToList() : new List<ACCredentialHolderSearchResult>());
		PartialResult = partialResult;
	}
}

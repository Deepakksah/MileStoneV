namespace VideoOS.Platform.Search.Errors;

public class InvalidFilterSearchError : SearchError
{
	public string FilterName { get; private set; }

	public string FilterValue { get; private set; }

	public InvalidFilterSearchError(string filterName, string filterValues)
	{
		FilterName = filterName;
		FilterValue = filterValues;
	}
}

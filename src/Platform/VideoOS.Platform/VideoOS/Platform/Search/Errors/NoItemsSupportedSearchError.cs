namespace VideoOS.Platform.Search.Errors;

public class NoItemsSupportedSearchError : SearchError
{
	public NoItemsSupportedSearchError()
	{
		base.ErrorType = SearchErrorType.Warning;
	}
}

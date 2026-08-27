namespace VideoOS.Platform.Search.Errors;

public abstract class SearchError
{
	public string Message { get; protected set; }

	public SearchErrorType ErrorType { get; protected set; }

	public SearchError()
	{
	}
}

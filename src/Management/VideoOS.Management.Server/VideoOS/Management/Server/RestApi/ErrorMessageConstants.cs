namespace VideoOS.Management.Server.RestApi;

public static class ErrorMessageConstants
{
	public const string InvalidId = "Invalid id";

	public const string InvalidFilterValue = "Invalid filter value";

	public const string AllFieldsNeedsToBeFilled = "All fields need to be filled, missing: {0}";

	public const string InvalidRecordingServerPath = "Parameter: recordingServerPath not formatted correctly";

	public const string TaskNotFound = "Task not found";

	public const string MethodFlowMissingNextMethod = "Method flow missing next MethodId";

	public const string EmptyId = "Empty id";

	public const string BadRequestWithMessage = "Bad request: {0} ";

	public const string KeyNotFoundWithKey = "Key not found: {0}";

	public const string InvalidGuidFormat = "Invalid GUID format";

	public const string UnknownResourceWithResourceName = "Unknown resource: {0}";

	public const string NoContent = "No content";

	public const string ElementGroupNotFound = "Element group not found";

	public const string MissingValueWithValue = "Missing value: {0}";

	public const string MissingParameters = "parameters missing";
}

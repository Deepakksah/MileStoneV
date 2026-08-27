namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

public sealed class MapCommandFaultExceptionErrorReason : CommonCommandFaultExceptionErrorReason
{
	private static string _referenceExists = "ReferenceExists";

	private static string _timestamp = "Timestamp";

	private static string _invalidId = "InvalidId";

	private static string _invalidBounds = "InvalidBounds";

	private static string _markedForDeletion = "MarkedForDeletion";

	private static string _insufficientRights = "InsufficientRights";

	private static string _invalidServerId = "InvalidServerId";

	public static string ReferenceExists => _referenceExists;

	public static string Timestamp => _timestamp;

	public static string InvalidId => _invalidId;

	public static string InvalidBounds => _invalidBounds;

	public static string MarkedForDeletion => _markedForDeletion;

	public static string InsufficientRights => _insufficientRights;

	public static string InvalidServerId => _invalidServerId;

	private MapCommandFaultExceptionErrorReason()
	{
	}
}

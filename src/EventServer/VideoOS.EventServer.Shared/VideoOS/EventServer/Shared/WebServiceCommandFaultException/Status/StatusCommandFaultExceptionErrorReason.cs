namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Status;

public sealed class StatusCommandFaultExceptionErrorReason : CommonCommandFaultExceptionErrorReason
{
	private static string _invalidVisualizationState = "InvalidVisualizationState";

	public static string InvalidVisualizationState => _invalidVisualizationState;

	private StatusCommandFaultExceptionErrorReason()
	{
	}
}

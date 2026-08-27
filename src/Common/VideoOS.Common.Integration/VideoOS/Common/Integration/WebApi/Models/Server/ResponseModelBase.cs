namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ResponseModelBase
{
	public ApplicationErrorCode ErrorCode { get; set; }

	public string Message { get; set; }
}

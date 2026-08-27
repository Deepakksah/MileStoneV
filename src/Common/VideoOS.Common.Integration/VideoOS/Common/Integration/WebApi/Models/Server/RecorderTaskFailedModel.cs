namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class RecorderTaskFailedModel
{
	public string Description { get; set; }

	public bool IsFatal { get; set; }

	public string ExceptionMessage { get; set; }
}

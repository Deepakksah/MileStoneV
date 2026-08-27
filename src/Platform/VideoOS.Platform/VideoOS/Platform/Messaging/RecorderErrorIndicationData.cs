namespace VideoOS.Platform.Messaging;

public class RecorderErrorIndicationData
{
	public string UrlString { get; set; }

	public string Command { get; set; }

	public int RecorderWebInterfaceErrorCode { get; set; }

	public int PtzErrorDetail { get; set; }

	public string ErrorText { get; set; }
}

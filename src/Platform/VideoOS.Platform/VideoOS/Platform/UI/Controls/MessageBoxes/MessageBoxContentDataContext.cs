namespace VideoOS.Platform.UI.Controls.MessageBoxes;

public class MessageBoxContentDataContext
{
	public string Header { get; }

	public string Message { get; }

	public VideoOSIconSourceBase IconSource { get; }

	public MessageBoxContentDataContext(string header, string message, VideoOSIconSourceBase iconSource)
	{
		Header = header;
		Message = message;
		IconSource = iconSource;
	}
}

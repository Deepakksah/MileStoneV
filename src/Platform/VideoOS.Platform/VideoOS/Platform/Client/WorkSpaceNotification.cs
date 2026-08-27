namespace VideoOS.Platform.Client;

public class WorkSpaceNotification
{
	public static readonly int SEVERITY_LOW = 0;

	public static readonly int SEVERITY_MEDIUM = 1;

	public static readonly int SEVERITY_HIGH = 2;

	public int Severity { get; private set; }

	public int Count { get; private set; }

	public string Text { get; private set; }

	public WorkSpaceNotification(int severity, int count, string text)
	{
		Severity = severity;
		Count = count;
		Text = text;
	}
}

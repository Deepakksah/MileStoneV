namespace VideoOS.Platform.UI.Automation.Serialization;

public class AutomationObjectSerializationPath
{
	public string OriginalPattern { get; }

	public string Pattern { get; private set; }

	public AutomationObjectSerializationPath(string pattern)
	{
		OriginalPattern = (Pattern = pattern);
	}

	public AutomationObjectSerializationPath Update(string remainder)
	{
		Pattern = remainder;
		return this;
	}
}

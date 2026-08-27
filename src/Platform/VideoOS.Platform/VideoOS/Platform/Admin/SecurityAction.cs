namespace VideoOS.Platform.Admin;

public class SecurityAction
{
	public string ActionId { get; set; }

	public string DisplayName { get; set; }

	public SecurityAction(string actionId, string displayName)
	{
		ActionId = actionId;
		DisplayName = displayName;
	}
}

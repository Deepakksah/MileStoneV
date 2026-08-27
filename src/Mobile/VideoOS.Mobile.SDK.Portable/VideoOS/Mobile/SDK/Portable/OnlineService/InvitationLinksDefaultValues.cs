namespace VideoOS.Mobile.SDK.Portable.OnlineService;

public class InvitationLinksDefaultValues
{
	public static InvitationLinksDefaultValues Instance = new InvitationLinksDefaultValues();

	public string AppLink { get; set; }

	public string StoreLink { get; set; }

	private InvitationLinksDefaultValues()
	{
		AppLink = "";
		StoreLink = "";
	}
}

namespace VideoOS.Platform.EventsData;

public class SmartWallEventChangedData : IEventChangedData<SmartWallChangedEventDataRelatedItem>
{
	public SmartWallChangedEventDataRelatedItem RelatedItem { get; set; }

	public string Action { get; set; }
}

namespace VideoOS.Platform.EventsData;

public class SystemConfigurationChangedEventData : IEventChangedData<SystemConfigurationChangedEventDataRelatedItem>
{
	public SystemConfigurationChangedEventDataRelatedItem RelatedItem { get; set; }

	public string Action { get; set; }
}

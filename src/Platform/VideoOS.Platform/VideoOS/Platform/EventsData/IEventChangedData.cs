namespace VideoOS.Platform.EventsData;

public interface IEventChangedData<T>
{
	T RelatedItem { get; set; }

	string Action { get; set; }
}

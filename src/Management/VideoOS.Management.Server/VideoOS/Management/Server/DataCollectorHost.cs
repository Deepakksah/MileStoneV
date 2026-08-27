namespace VideoOS.Management.Server;

public class DataCollectorHost
{
	public string HostName { get; set; }

	public string ApplicationId { get; set; }

	public int DataCollectorId { get; set; }

	public static DataCollectorHost[] GetDataCollectorHosts(IGeminiRequestContext requestContext, int dataCollectorId)
	{
		using DataCollectionComponent dataCollectionComponent = requestContext.CreateComponent<DataCollectionComponent>();
		return dataCollectionComponent.GetDataCollectorHosts(dataCollectorId);
	}
}

namespace VideoOS.Management.Server.RestApi;

public class RegistrationServiceData
{
	public string name { get; set; }

	public string description { get; set; }

	public string id { get; set; }

	public string serviceType { get; set; }

	public bool? enabled { get; set; }

	public string customData { get; set; }

	public RegistrationServiceEndpoint[] endpoints { get; set; }

	public string[] uris { get; set; }

	public bool? trusted { get; set; }

	public string dataCollectorId { get; set; }
}

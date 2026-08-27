namespace VideoOS.Platform.SDK.Config;

public class Address
{
	public string Url { get; set; }

	public string Type { get; set; }

	public Address()
	{
	}

	public Address(string url, string type = "Server")
	{
		Url = url;
		Type = type;
	}
}

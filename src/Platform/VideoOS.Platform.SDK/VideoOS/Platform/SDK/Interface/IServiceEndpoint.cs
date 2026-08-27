namespace VideoOS.Platform.SDK.Interface;

public interface IServiceEndpoint
{
	string authentication { get; set; }

	string description { get; set; }

	string name { get; set; }

	string uri { get; set; }
}

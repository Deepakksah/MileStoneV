namespace VideoOS.Mobile.ReverseProxy;

public class IdentityPathMapper : IPathMapper
{
	public string Map(string path)
	{
		return path;
	}
}

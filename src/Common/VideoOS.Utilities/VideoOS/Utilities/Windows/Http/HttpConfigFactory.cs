using VideoOS.Utilities.Windows.Http.Internal;

namespace VideoOS.Utilities.Windows.Http;

public sealed class HttpConfigFactory : IHttpSysConfigFactory
{
	public static IHttpSysConfigFactory Instance { get; } = new HttpConfigFactory();

	private HttpConfigFactory()
	{
	}

	IHttpSysConfigReader IHttpSysConfigFactory.CreateConfigReader()
	{
		return new HttpSysConfig();
	}

	IHttpSysConfig IHttpSysConfigFactory.CreateConfig()
	{
		return new HttpSysConfig();
	}
}

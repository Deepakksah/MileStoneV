using VideoOS.Platform.Log;

namespace VideoOS.Platform.SDK;

public interface ILogClientFactory
{
	LogClient GetLogByContext(UserContext userContext);
}

using System;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpListener : IDisposable
{
	IHttpListenerPrefixCollection Prefixes { get; }

	Task<IHttpListenerContext> GetContextAsync();

	void Start();

	void Stop();
}

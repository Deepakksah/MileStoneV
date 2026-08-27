using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.LogServer.Communication;

public interface IClientWrapper<T> where T : ICommunicationObject
{
	Task<TRes> CallWithRetryAsync<TRes>(Func<T, Task<TRes>> func);
}

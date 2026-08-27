using MediatR;
using VideoOS.DataCollector.Service.Common.Models;

namespace VideoOS.DataCollector.Service.Common;

public interface ISendRequest<out TResponse> : IRequest<TResponse>, IBaseRequest
{
	RetrySettings GetRetrySettings();
}

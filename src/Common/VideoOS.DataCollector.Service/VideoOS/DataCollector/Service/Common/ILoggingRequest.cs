using MediatR;
using VideoOS.DataCollector.Service.Common.Models;

namespace VideoOS.DataCollector.Service.Common;

public interface ILoggingRequest<out TResponse> : IRequest<TResponse>, IBaseRequest
{
	LogSettings GetLogSettings();
}

using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnection
{
	IAsyncResult SendCommand(Command command, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null, string address = null);

	IAsyncResult SendCommand<T>(Command command, OnResponse<T> onSuccess, OnResponse<T> onFail, Func<BaseCommandResponse, T> factoryT, string address = null) where T : BaseCommandResponse;

	BaseCommandResponse ProcessResult(IAsyncResult iResult, TimeSpan timeout);

	Task<BaseCommandResponse> ProcessResultTask(IAsyncResult asyncResult, TimeSpan timeout);
}

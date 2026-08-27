using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IStreamControlAwait<T> where T : StreamParams
{
	Task<BaseCommandResponse> ChangeStreamAsync(T streamParams, TimeSpan timeout);
}

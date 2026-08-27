using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IStreamControlSync<T> where T : StreamParams
{
	BaseCommandResponse ChangeStream(T streamParams, TimeSpan timeout);
}

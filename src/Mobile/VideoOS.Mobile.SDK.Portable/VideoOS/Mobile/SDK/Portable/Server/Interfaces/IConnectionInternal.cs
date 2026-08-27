using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IConnectionInternal
{
	IAsyncResult SendCommandInternal(Command tCommand, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null, CommandAsyncResultBase processor = null, string address = null);
}

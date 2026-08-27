using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ITwoStepVerificationAsync
{
	IAsyncResult RequestSecondStepVerificationCode(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult VerifySecondStepVerificationCode(string code, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}

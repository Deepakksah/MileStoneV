using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface ITwoStepVerificationSync
{
	BaseCommandResponse RequestSecondStepVerificationCode(TimeSpan timeout);

	BaseCommandResponse VerifySecondStepVerificationCode(string code, TimeSpan timeout);
}

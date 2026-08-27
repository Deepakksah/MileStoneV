using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IErrorResult
{
	Command.CommandResultTypes Result { get; }

	string ErrorMessage { get; }

	ErrorCodes ErrorCode { get; }
}

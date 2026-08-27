using System;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IErrorHandlers
{
	void SubscribeToError(ErrorCodes errorCode, EventHandler errorHandler);

	void UnsubscribeFromError(ErrorCodes errorCode, EventHandler errorHandler);
}

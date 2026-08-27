using System;

namespace VideoOS.Common.Interfaces.Recorder;

public interface IPtzExtensionCallback
{
	PtzCallbackResult LockPtzSession(Guid deviceId, string token);

	PtzCallbackResult UnlockPtzSession(Guid deviceId, string token);

	PtzCallbackResult ForceUnlockPtzSession(Guid deviceId, string token);
}

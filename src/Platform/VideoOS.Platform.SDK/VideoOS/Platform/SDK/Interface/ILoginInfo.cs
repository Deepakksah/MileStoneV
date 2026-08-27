using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ILoginInfo
{
	string Token { get; set; }

	DateTime RegistrationTime { get; set; }

	ITimeDuration TimeToLive { get; set; }

	bool TimeToLiveLimited { get; set; }
}

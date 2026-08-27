using System;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication;

public class LoginSettingsNotFoundException : Exception
{
	public ServerId ServerId { get; }

	public LoginSettingsNotFoundException(ServerId serverId)
	{
		ServerId = serverId;
	}

	public LoginSettingsNotFoundException()
	{
	}

	public LoginSettingsNotFoundException(string message)
		: base(message)
	{
	}

	public LoginSettingsNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

using System;

namespace VideoOS.Reporting.Web.Plugin.Communication;

public class SecurityToken
{
	private TimeSpan _expires;

	private DateTime _registrationTime;

	public bool UpdatePending { get; set; }

	public string Token { get; set; }

	public bool IsCloseToExpire => DateTime.UtcNow > _registrationTime + _expires - new TimeSpan(_expires.Ticks / 2);

	public SecurityToken(string token, DateTime registrationTime, TimeSpan expires)
	{
		_expires = expires;
		_registrationTime = registrationTime;
		Token = token;
	}
}

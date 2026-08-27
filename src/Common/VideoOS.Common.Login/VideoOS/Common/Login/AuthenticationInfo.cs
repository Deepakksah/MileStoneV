using System;

namespace VideoOS.Common.Login;

public abstract class AuthenticationInfo
{
	public virtual string Name
	{
		get
		{
			throw new InvalidOperationException("Get Name not supported.");
		}
		set
		{
			throw new InvalidOperationException("Set Name not supported.");
		}
	}

	public abstract string LoginProviderId { get; set; }

	public virtual bool Enabled
	{
		get
		{
			return true;
		}
		set
		{
			throw new InvalidOperationException("Set Enabled not supported.");
		}
	}

	public abstract AuthenticationType Type { get; }

	public abstract string UserLabel { get; }

	public abstract string PasswordLabel { get; }

	public abstract bool SupportUserName { get; }

	public abstract bool SupportPassword { get; }

	public abstract string AutomationId { get; }

	public override string ToString()
	{
		return Name;
	}
}

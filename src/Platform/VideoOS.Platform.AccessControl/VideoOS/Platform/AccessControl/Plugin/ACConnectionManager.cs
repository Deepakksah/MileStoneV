using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACConnectionManager
{
	public event EventHandler<ACSystemConnectionStateChangedEventArgs> ACSystemConnectionStateChanged = delegate
	{
	};

	public abstract void Connect();

	public abstract void Disconnect();

	public virtual ACUserCredentialsValidationResult ValidateUserCredentials(string username, string password)
	{
		throw new NotImplementedException();
	}

	protected void FireACSystemConnectionStateChanged(bool connected)
	{
		this.ACSystemConnectionStateChanged(this, new ACSystemConnectionStateChangedEventArgs(connected));
	}
}

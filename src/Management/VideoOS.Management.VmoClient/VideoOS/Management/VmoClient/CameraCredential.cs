using System;

namespace VideoOS.Management.VmoClient;

public class CameraCredential
{
	private string _userName;

	private string _password;

	public string UserName
	{
		get
		{
			return _userName;
		}
		set
		{
			_userName = value;
			OnChanged();
		}
	}

	public string Password
	{
		get
		{
			return _password;
		}
		set
		{
			_password = value;
			OnChanged();
		}
	}

	public event EventHandler ChangedEvent;

	public virtual void OnChanged()
	{
		this.ChangedEvent?.Invoke(this, new EventArgs());
	}

	public CameraCredential(string userName, string password)
	{
		_userName = userName;
		_password = password;
	}
}

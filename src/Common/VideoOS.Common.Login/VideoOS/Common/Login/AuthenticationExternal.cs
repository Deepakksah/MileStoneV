using System;

namespace VideoOS.Common.Login;

public class AuthenticationExternal : AuthenticationInfo
{
	private string _name;

	private string _loginProviderId;

	private bool _enabled;

	private string _userLabel;

	private string _passwordLabel;

	public override string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value;
		}
	}

	public override string LoginProviderId
	{
		get
		{
			return _loginProviderId;
		}
		set
		{
			_loginProviderId = value;
		}
	}

	public override bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public override AuthenticationType Type => AuthenticationType.External;

	public override string UserLabel => _userLabel;

	public override string PasswordLabel => _passwordLabel;

	public override bool SupportUserName => false;

	public override bool SupportPassword => false;

	public override string AutomationId => "_externalUser";

	public AuthenticationExternal(string userLabel, string passwordLabel)
	{
		_userLabel = userLabel;
		_passwordLabel = passwordLabel;
	}

	public AuthenticationExternal(AuthenticationExternal authenticationExternal)
	{
		if (authenticationExternal == null)
		{
			throw new ArgumentNullException("authenticationExternal");
		}
		_name = authenticationExternal.Name;
		_loginProviderId = authenticationExternal.LoginProviderId;
		_enabled = authenticationExternal.Enabled;
		_userLabel = authenticationExternal.UserLabel;
		_passwordLabel = authenticationExternal.PasswordLabel;
	}
}

using System;
using VideoOS.Common.Definition;

namespace VideoOS.Common.Login;

public class DialogLoginResult
{
	private readonly LoginResults _loginResult;

	public virtual LoginResults LoginResult => _loginResult;

	public bool ShowDialog { get; private set; }

	public virtual Exception Exception => new LoginException(Message);

	public string Message { get; private set; }

	public WpfMessageBoxImage MessageBoxIcon { get; private set; }

	public bool ClearPassword { get; private set; }

	public DialogLoginResult(LoginResults loginResult)
	{
		_loginResult = loginResult;
		Message = null;
		ShowDialog = false;
	}

	public DialogLoginResult(LoginResults loginResult, string message)
	{
		_loginResult = loginResult;
		Message = message;
		MessageBoxIcon = WpfMessageBoxImage.Information;
		ShowDialog = true;
	}

	public DialogLoginResult(LoginResults loginResult, string message, WpfMessageBoxImage icon)
	{
		_loginResult = loginResult;
		Message = message;
		MessageBoxIcon = icon;
		ShowDialog = true;
	}

	public DialogLoginResult(LoginResults loginResult, string message, WpfMessageBoxImage icon, bool clearPassword)
	{
		_loginResult = loginResult;
		Message = message;
		MessageBoxIcon = icon;
		ClearPassword = clearPassword;
		ShowDialog = true;
	}
}

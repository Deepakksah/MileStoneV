namespace VideoOS.Common.Login;

public class AuthenticationWindowsDefault : AuthenticationInfo
{
	private readonly string _passwordLabel;

	private readonly string _userLabel;

	public override string Name => Resources.ServerTypeNameWindowsDefault;

	public override string LoginProviderId { get; set; }

	public override bool Enabled => true;

	public override AuthenticationType Type => AuthenticationType.WindowsDefault;

	public override string UserLabel => _userLabel;

	public override string PasswordLabel => _passwordLabel;

	public override bool SupportUserName => false;

	public override bool SupportPassword => false;

	public override string AutomationId => "_currentUser";

	public AuthenticationWindowsDefault(string userLabel, string passwordLabel)
	{
		_userLabel = userLabel;
		_passwordLabel = passwordLabel;
	}
}

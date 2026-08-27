namespace VideoOS.Common.Login;

public class AuthenticationSimple : AuthenticationInfo
{
	private readonly string _passwordLabel;

	private readonly string _userLabel;

	public override string Name => Resources.ServerTypeNameSimple;

	public override string LoginProviderId { get; set; }

	public override bool Enabled => true;

	public override AuthenticationType Type => AuthenticationType.Simple;

	public override string UserLabel => _userLabel;

	public override string PasswordLabel => _passwordLabel;

	public override bool SupportUserName => true;

	public override bool SupportPassword => true;

	public override string AutomationId => "_basicUser";

	public AuthenticationSimple(string userLabel, string passwordLabel)
	{
		_userLabel = userLabel;
		_passwordLabel = passwordLabel;
	}
}

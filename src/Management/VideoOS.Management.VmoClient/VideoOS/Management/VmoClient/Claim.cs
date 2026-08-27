using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("Claim, {LoginProviderId} : {Name} = {Value}")]
public class Claim
{
	public string LoginProviderId { get; }

	public string Name { get; }

	public string Value { get; }

	public Claim(string loginProviderId, string name, string value)
	{
		LoginProviderId = loginProviderId;
		Name = name;
		Value = value;
	}
}

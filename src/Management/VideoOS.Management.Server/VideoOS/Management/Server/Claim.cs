using System;
using System.Diagnostics;

namespace VideoOS.Management.Server;

[DebuggerDisplay("Claim, {LoginProviderId} : {Name} = {Value}")]
public class Claim
{
	public Guid LoginProviderId { get; }

	public string Name { get; }

	public string Value { get; }

	public Claim(Guid loginProviderId, string name, string value)
	{
		LoginProviderId = loginProviderId;
		Name = name;
		Value = value;
	}
}

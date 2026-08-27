using System;
using System.Diagnostics;

namespace VideoOS.Management.Server;

[DebuggerDisplay("RegisteredClaimData, {Id}; {LoginProviderId.ToString(),nq}; {Name,nq}")]
public class RegisteredClaimData
{
	public int Id { get; set; }

	public Guid LoginProviderId { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public bool CaseSensitive { get; set; }
}

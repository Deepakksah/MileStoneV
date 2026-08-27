using System.Diagnostics;

namespace VideoOS.Management.Server;

[DebuggerDisplay("RegisteredClaimData, {Id}; {LoginProviderId,nq}; {Name,nq}")]
public class RegisteredClaimNotValidated
{
	public int Id { get; set; }

	public string LoginProviderId { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public bool? CaseSensitive { get; set; }
}

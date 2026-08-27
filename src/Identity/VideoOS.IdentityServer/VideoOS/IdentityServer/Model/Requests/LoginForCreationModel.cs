using System.ComponentModel.DataAnnotations;
using VideoOS.IdentityServer.Attributes;

namespace VideoOS.IdentityServer.Model.Requests;

public class LoginForCreationModel
{
	[Required]
	[IsGuid]
	public string UserId { get; set; }

	[Required]
	public string LoginProviderId { get; set; }

	[Required]
	public string ProviderKey { get; set; }

	public string ProviderDisplayName { get; set; }
}

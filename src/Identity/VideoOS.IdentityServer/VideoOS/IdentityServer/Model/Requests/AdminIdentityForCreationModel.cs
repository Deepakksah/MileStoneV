using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Model.Requests;

public class AdminIdentityForCreationModel
{
	[Required]
	[MaxLength(450)]
	public string Identity { get; set; }
}

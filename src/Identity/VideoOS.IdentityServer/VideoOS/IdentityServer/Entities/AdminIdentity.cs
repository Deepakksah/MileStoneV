using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.Entities;

public class AdminIdentity
{
	[Key]
	public int Id { get; set; }

	[MaxLength(450)]
	[Required]
	public string Identity { get; set; }
}

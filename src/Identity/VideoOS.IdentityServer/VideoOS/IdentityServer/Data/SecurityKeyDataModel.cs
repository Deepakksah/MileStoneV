using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoOS.IdentityServer.Data;

[Table("SecurityKey")]
public class SecurityKeyDataModel
{
	[Key]
	public int Id { get; set; }

	[MaxLength(200)]
	public string KeyId { get; set; }

	public DateTime? Expiration { get; set; }

	public DateTime Created { get; set; }

	[MaxLength(256)]
	public byte[] D { get; set; }

	[MaxLength(128)]
	public byte[] DP { get; set; }

	[MaxLength(128)]
	public byte[] DQ { get; set; }

	[MaxLength(3)]
	public byte[] Exponent { get; set; }

	[MaxLength(128)]
	public byte[] InverseQ { get; set; }

	[MaxLength(256)]
	public byte[] Modulus { get; set; }

	[MaxLength(128)]
	public byte[] P { get; set; }

	[MaxLength(128)]
	public byte[] Q { get; set; }
}

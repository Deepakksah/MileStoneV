namespace VideoOS.IdentityServer.Configuration;

public class PasswordSettings
{
	public bool RequireDigit { get; set; } = true;

	public bool RequireLowercase { get; set; } = true;

	public bool RequireNonAlphanumeric { get; set; } = true;

	public bool RequireUppercase { get; set; } = true;

	public int RequiredLength { get; set; } = 8;

	public int RequiredUniqueChars { get; set; } = 1;

	public int MaxLength { get; set; } = 255;
}

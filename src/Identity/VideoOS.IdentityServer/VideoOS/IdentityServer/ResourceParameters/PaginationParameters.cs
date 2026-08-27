using System.ComponentModel.DataAnnotations;

namespace VideoOS.IdentityServer.ResourceParameters;

public class PaginationParameters
{
	[Required]
	[Range(1, int.MaxValue)]
	public int? Top { get; set; }

	[Required]
	[Range(0, int.MaxValue)]
	public int? Skip { get; set; }

	public PaginationParameters()
	{
	}

	public PaginationParameters(int skip, int top)
	{
		Skip = skip;
		Top = top;
	}
}

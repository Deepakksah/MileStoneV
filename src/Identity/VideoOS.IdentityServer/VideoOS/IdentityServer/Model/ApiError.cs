using Microsoft.AspNetCore.Identity;

namespace VideoOS.IdentityServer.Model;

public class ApiError
{
	public string Code { get; set; }

	public string Description { get; set; }

	public ApiError()
	{
	}

	public ApiError(IdentityError error)
	{
		Code = error.Code;
		Description = error.Description;
	}
}

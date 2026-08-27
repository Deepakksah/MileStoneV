using System;
using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class UserNotFoundException : NotFoundException
{
	public UserNotFoundException(ErrorResponse errors)
		: base(errors)
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
	}
}

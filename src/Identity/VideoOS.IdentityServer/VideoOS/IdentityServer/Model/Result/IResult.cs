using System.Collections.Generic;

namespace VideoOS.IdentityServer.Model.Result;

public interface IResult
{
	IReadOnlyList<Error> Errors { get; }

	bool IsError { get; }
}

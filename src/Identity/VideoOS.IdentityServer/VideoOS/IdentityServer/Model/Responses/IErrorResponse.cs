using System.Collections.Generic;

namespace VideoOS.IdentityServer.Model.Responses;

public interface IErrorResponse
{
	IReadOnlyDictionary<string, string[]> Errors { get; set; }
}

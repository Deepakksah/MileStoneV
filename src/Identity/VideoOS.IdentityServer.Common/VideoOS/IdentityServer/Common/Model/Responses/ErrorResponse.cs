using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.IdentityServer.Common.Model.Responses;

public class ErrorResponse
{
	public IReadOnlyDictionary<string, string[]> Errors { get; set; }

	public string Error { get; set; }

	[JsonProperty("error_code")]
	public string Code { get; set; }

	[JsonProperty("extensions")]
	public IReadOnlyDictionary<string, object> Extensions { get; set; }
}

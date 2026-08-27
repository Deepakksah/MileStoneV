using System;

namespace VideoOS.Common.Integration.WebApi;

public class ResponseModel
{
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }
}

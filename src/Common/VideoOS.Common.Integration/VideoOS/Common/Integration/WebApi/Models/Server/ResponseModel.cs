using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class ResponseModel
{
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }
}

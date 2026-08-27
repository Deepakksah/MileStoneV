using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public abstract class CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid Token { get; set; }
}

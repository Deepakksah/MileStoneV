using System;

namespace VideoOS.Common.Integration.Command;

public interface ICommandWithSource
{
	Guid SourceId { get; }
}

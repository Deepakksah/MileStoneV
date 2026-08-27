using System;

namespace VideoOS.Common.Integration.Command;

public interface IVisitableCommand
{
	Guid Token { get; }

	void Accept(ICommandVisitor visitor);
}

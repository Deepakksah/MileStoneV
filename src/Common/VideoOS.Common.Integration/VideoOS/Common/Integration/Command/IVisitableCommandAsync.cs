using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public interface IVisitableCommandAsync
{
	Guid Token { get; }

	Task AcceptAsync(ICommonCommandVisitorAsync visitor);
}

using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public interface ICommandContainer
{
	CommandBase GetCommand(Guid token);
}

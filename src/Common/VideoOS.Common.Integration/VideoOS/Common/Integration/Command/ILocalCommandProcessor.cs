using System;

namespace VideoOS.Common.Integration.Command;

public interface ILocalCommandProcessor
{
	IResponseCollector ProcessCommandAsync<TCommand>(TCommand command, object state) where TCommand : CommandBase;

	void ProcessCommandAsync<TCommand>(TCommand command, object state, Action<TCommand, ResponseBase> responseAction) where TCommand : CommandBase;
}

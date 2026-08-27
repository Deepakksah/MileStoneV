using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Server.CommandHandler;

public sealed class CleanupHardwareDefinitionsCommandHandler : ICommandHandler
{
	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is CleanupHardwareDefinitionsCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		ConfigurationDataSet configurationDataSet = Server.Instance.Data.LoadHardwareDefinitionsUnused();
		if (configurationDataSet.HardwareDefinition.Count > 0)
		{
			foreach (ConfigurationDataSet.HardwareDefinitionRow row in configurationDataSet.HardwareDefinition.Rows)
			{
				row.Delete();
			}
			ConfigurationDataSet changes = configurationDataSet.GetChanges();
			ConfigurationManagerDataTransferObject configurationManagerDto = new ConfigurationManagerDataTransferObject(changes);
			UpdateConfigurationManagerCommand updateConfigurationManagerCommand = new UpdateConfigurationManagerCommand(configurationManagerDto);
			updateConfigurationManagerCommand.PersistChanges = true;
			ICallContextInfo state2 = Server.Instance.CreateServerServiceCallContextUserInfo();
			CommandProcessor.ProcessSync(updateConfigurationManagerCommand, state2, new NullResponseSink());
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}

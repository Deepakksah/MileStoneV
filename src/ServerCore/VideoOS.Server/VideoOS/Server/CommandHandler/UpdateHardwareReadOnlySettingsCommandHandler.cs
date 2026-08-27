using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Exceptions;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateHardwareReadOnlySettingsCommandHandler : ICommandHandler
{
	private Server _server;

	public UpdateHardwareReadOnlySettingsCommandHandler(Server server)
	{
		_server = server;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateHardwareReadOnlySettingsCommand updateHardwareReadOnlySettingsCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			ConfigurationDataSet configurationDataSet = null;
			_server.Data.ConfigurationDataSetCacheLock.EnterWriteLock();
			Guid iDRecorder;
			try
			{
				using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data)))
				{
					IPHardware iPHardware;
					try
					{
						iPHardware = configurationManager.FindHardwareByHardwareId(updateHardwareReadOnlySettingsCommand.HardwareId);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception)
					{
						throw new HardwareApplicationException("Failed to load specified hardware. Id: " + updateHardwareReadOnlySettingsCommand.HardwareId.ToString());
					}
					iDRecorder = iPHardware.IDRecorder;
					string xml = DefinitionUtility.InsertTypeAttributeInHardwareSettingXml(updateHardwareReadOnlySettingsCommand.UpdatedSettingsXml, iPHardware.DefinitionAsXml, refresh: false);
					iPHardware.Settings.LoadXml(xml, SettingsLoadMethod.Update);
					iPHardware.LastModified = DateTime.UtcNow;
					configurationDataSet = configurationManager.Dataset.GetChanges();
					ConfigurationDataSet changes = (ConfigurationDataSet)configurationDataSet.Copy();
					_server.Data.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
				}
				ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
				requiredService.ClearAllCaches();
				byte[] array = _server.RequestRecorderLicense(iDRecorder);
				using ConfigurationManager configurationManager2 = new ConfigurationManager(new ConfigurationDataSetLoader(_server.Data));
				License license = null;
				try
				{
					license = configurationManager2.Licenses[iDRecorder];
				}
				catch (ConfigurationItemNotFoundException)
				{
				}
				if (license == null)
				{
					configurationManager2.Licenses.Add(iDRecorder, array);
				}
				else
				{
					license.DataAsBytes = array;
					license.LastModified = DateTime.UtcNow;
				}
				ConfigurationDataSet changes2 = configurationManager2.Dataset.GetChanges();
				configurationDataSet.Merge(changes2);
				ICallContextInfo callContextInfo2 = Server.Instance.CreateServerServiceCallContextUserInfo();
				_server.Data.WriteConfigurationDataSetChangesToDatabase(changes2, callContextInfo2);
			}
			finally
			{
				_server.Data.ConfigurationDataSetCacheLock.ExitWriteLock();
			}
			responseSink.AddResponse(new SuccessResponse());
			Guid[] array2 = _server.RecorderCommManager.QueryRecorderServiceIds(iDRecorder);
			foreach (Guid serviceId in array2)
			{
				try
				{
					_server.RecorderCommManager.UpdateSystemConfigurationAsync(serviceId, new SystemConfigurationDto(configurationDataSet), callContextInfo);
				}
				catch (RecorderNotFoundException)
				{
				}
				catch (NotSupportedByLegacyRecorderException)
				{
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex7)
		{
			string resourceString = ExceptionHandling.GetResourceString(ex7);
			if (string.IsNullOrEmpty(resourceString))
			{
				responseSink.AddResponse(new FailResponse());
			}
			else
			{
				responseSink.AddResponse(new ErrorResponse(0, resourceString));
			}
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}

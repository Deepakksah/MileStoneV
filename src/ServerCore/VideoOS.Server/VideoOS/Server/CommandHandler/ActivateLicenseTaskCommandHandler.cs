using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient;
using VideoOS.Server.Properties;

namespace VideoOS.Server.CommandHandler;

public class ActivateLicenseTaskCommandHandler : ICommandHandler
{
	private IServerEnvironment _serverEnviroment;

	private ILicenseManager LicenseManager => _serverEnviroment.LicenseManager;

	private IVmoClientFactory VmoClientFactory => _serverEnviroment.VmoClientFactory;

	private ILogger Logger => _serverEnviroment.Server.Logger;

	public ActivateLicenseTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		_serverEnviroment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ActivateLicenseTaskCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ResponseBase response = ProcessCommand(cmd, responseSink);
		responseSink.AddResponse(response);
	}

	private ResponseBase ProcessCommand(ActivateLicenseTaskCommand cmd, IResponseSink responseSink)
	{
		DateTime utcNow = DateTime.UtcNow;
		int percentComplete = 0;
		responseSink.AddResponse(new TaskProgressResponse(utcNow, percentComplete));
		try
		{
			LicenseManager.ActivateLicense(cmd.UserName, cmd.Password);
		}
		catch (LicenseException ex)
		{
			return HandleLicenseException((LicenseErrorNumber)ex.ErrorNumber);
		}
		Task.Run(delegate
		{
			VmoClientsPool.VmoTimedClient connectedClient = VmoClientFactory.GetConnectedClient();
			try
			{
				connectedClient.ManagementServer.RefreshLicenseCache();
			}
			catch (VmoException ex2)
			{
				Logger.LogError("Unable to refresh IIS license cache after activation: {ExceptionMessage}", ex2.Message);
			}
			finally
			{
				VmoClientFactory.ReleaseClient(connectedClient);
			}
		});
		return new TaskSuccessResponse();
	}

	private static ResponseBase HandleLicenseException(LicenseErrorNumber errorNumber)
	{
		TaskError taskError = new TaskError();
		switch (errorNumber)
		{
		case LicenseErrorNumber.Expired:
			taskError.ErrorCode = 64003;
			taskError.ErrorText = VmoResources.VMO64003;
			break;
		case LicenseErrorNumber.LicenseDecryptFailure:
		case LicenseErrorNumber.InvalidLicenseFile:
			taskError.ErrorCode = 64004;
			taskError.ErrorText = VmoResources.VMO64004;
			break;
		case LicenseErrorNumber.GeneralLicenseError:
			taskError.ErrorCode = 64000;
			taskError.ErrorText = VmoResources.VMO64000;
			break;
		case LicenseErrorNumber.LicenseActivationServerCommunicationFailed:
			taskError.ErrorCode = 64058;
			taskError.ErrorText = VmoResources.VMO64058;
			break;
		case LicenseErrorNumber.UserNameAndPasswordNotFound:
			taskError.ErrorCode = 64006;
			taskError.ErrorText = VmoResources.VMO64006;
			break;
		case LicenseErrorNumber.SlcNotFound:
			taskError.ErrorCode = 64007;
			taskError.ErrorText = VmoResources.VMO64007;
			break;
		case LicenseErrorNumber.SlcRegisteredOnOtherLogOn:
			taskError.ErrorCode = 64008;
			taskError.ErrorText = VmoResources.VMO64008;
			break;
		case LicenseErrorNumber.MismatchOnComputerSid:
			taskError.ErrorCode = 64009;
			taskError.ErrorText = VmoResources.VMO64009;
			break;
		case LicenseErrorNumber.AccessDenied:
			taskError.ErrorCode = 64050;
			taskError.ErrorText = VmoResources.VMO64050;
			break;
		case LicenseErrorNumber.TooManyCamerasRequested:
			taskError.ErrorCode = 64051;
			taskError.ErrorText = VmoResources.VMO64051;
			break;
		case LicenseErrorNumber.TooManyHardwareDevicesRequested:
			taskError.ErrorCode = 64052;
			taskError.ErrorText = VmoResources.VMO64052;
			break;
		case LicenseErrorNumber.TooManyInterconnectedCamerasRequested:
			taskError.ErrorCode = 64053;
			taskError.ErrorText = VmoResources.VMO64053;
			taskError.Arguments.Add(0, OemResources.Instance.Server.ICPApplicationName);
			break;
		case LicenseErrorNumber.TooManyRecordersRequested:
			taskError.ErrorCode = 64054;
			taskError.ErrorText = VmoResources.VMO64054;
			break;
		case LicenseErrorNumber.SlcNotRegistered:
			taskError.ErrorCode = 64055;
			taskError.ErrorText = VmoResources.VMO64055;
			break;
		case LicenseErrorNumber.SlcAlreadyInUse:
			taskError.ErrorCode = 64056;
			taskError.ErrorText = VmoResources.VMO64056;
			break;
		case LicenseErrorNumber.ActivationLimitExceeded:
			taskError.ErrorCode = 64057;
			taskError.ErrorText = VmoResources.VMO64057;
			break;
		case LicenseErrorNumber.CustomerIsNotCdsValidated:
			taskError.ErrorCode = 64059;
			taskError.ErrorText = VmoResources.VMO64059;
			break;
		default:
			taskError.ErrorCode = 64000;
			taskError.ErrorText = VmoResources.VMO64000;
			break;
		}
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}
}
